//
//  EtceteraTwoManager.m
//  Unity-iPhone
//
//  Created by Mike on 4/8/11.
//  Copyright 2011 Prime31 Studios. All rights reserved.
//

#import "EtceteraTwoManager.h"
#include <sys/xattr.h>


#if UNITY_VERSION < 500
void UnityPause( bool pause );
#else
void UnityPause( int pause );
#endif

void UnitySendMessage( const char * className, const char * methodName, const char * param );

UIViewController *UnityGetGLViewController();


UIColor * UIColorFromHex( int hexcolor )
{
	int r = ( hexcolor >> 24 ) & 0xFF;
	int g = ( hexcolor >> 16 ) & 0xFF;
	int b = ( hexcolor >> 8 ) & 0xFF;
	int a = hexcolor & 0xFF;
	
	if( a == 0 )
		a = 1.0f;
	
	return [UIColor colorWithRed:(r/255.0) green:(g/255.0) blue:(b/255.0) alpha:(a/255.0)];
}



@implementation EtceteraTwoManager

@synthesize screenManager;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (EtceteraTwoManager*)sharedManager
{
	static EtceteraTwoManager *sharedManager = nil;
	
	if( !sharedManager )
		sharedManager = [[EtceteraTwoManager alloc] init];
	
	return sharedManager;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark P31MoviePlayerViewControllerDelegate

- (void)moviePlayerControllerDidFinish:(P31MoviePlayerViewController*)player
{
	UnityPause( false );
	
	// kill the player
	[UnityGetGLViewController() dismissModalViewControllerAnimated:YES];
	UnitySendMessage( "EtceteraTwoManager", "moviePlayerDidFinish", "" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark ExternalScreenManagerDelegate

- (void)screenManagerDidStartMirroring
{
	UnitySendMessage( "EtceteraTwoManager", "screenMirroringDidStart", "" );
}


- (void)screenManagerDidStopMirroring
{
	UnitySendMessage( "EtceteraTwoManager", "screenMirroringDidStop", "" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Crack Detection

- (BOOL)isJailBroken
{
	NSArray *apps = [NSArray arrayWithObjects:@"/Applications/Cydia.app", @"/Applications/limera1n.app", @"/Applications/greenpois0n.app",
					 @"/Applications/blackra1n.app", @"/Applications/blacksn0w.app", @"/Applications/redsn0w.app", nil];
	
	// Now check for known jailbreak apps. If we encounter one, the device is jailbroken.
	for( NSString *app in apps )
	{
		if( [[NSFileManager defaultManager] fileExistsAtPath:app] )
			return YES;
	}
	
	return NO;
}


- (BOOL)isInfoPlistPatched
{
	char csignid[] = "PfdkboFabkqfqv";
	for( int i = 0; i < strlen( csignid ); i++ )
		csignid[i] = csignid[i] + 3;
	NSString *signIdentity = [[[NSString alloc] initWithCString:csignid encoding:NSUTF8StringEncoding] autorelease];
	
	if( [[[NSBundle mainBundle] infoDictionary] objectForKey:signIdentity] != nil )
		return YES;
	return NO;
}


- (BOOL)isCracked:(long)filesize
{
	if( [self isInfoPlistPatched] )
		return YES;
	
	// only check filesize if it was passed in
	if( filesize > 0 )
	{
		long actualFileSize = [self infoPlistFileSize];
		if( actualFileSize != filesize )
			return YES;
	}

	return NO;
}


- (long)infoPlistFileSize
{
	NSString *path = [[[NSBundle mainBundle] bundlePath] stringByAppendingPathComponent:@"Info.plist"];
	
	NSDictionary *fileAttributes = [[NSFileManager defaultManager] attributesOfItemAtPath:path error:nil];	
	if( fileAttributes != nil )
	{
		NSNumber *fileSize = [fileAttributes objectForKey:NSFileSize];
		NSLog( @"File size: %qi\n", [fileSize unsignedLongLongValue] );
		
		return [fileSize unsignedLongLongValue];
	}
	
	NSLog( @"could not find Info.plist file" );
	
	return 0;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)startListeningForExternalScreensWithFPS:(int)fps
{
	// early out if we already exist
	if( screenManager )
		return;
	
	screenManager = [[ExternalScreenManager alloc] init];
	screenManager.targetFPS = fps;
	screenManager.delegate = self;
	[screenManager listenForExternalScreens];
}


- (void)stopExternalScreenMirroring
{
	[screenManager stop];
	self.screenManager = nil;
}


- (void)setExternalScreenMirorringBackgroundColor:(uint)color
{
	if( screenManager )
		screenManager.backgroundColor = UIColorFromHex( color );
}


- (void)setExternalScreenMirorringScale:(CGFloat)scale
{
	if( screenManager )
		screenManager.scale = scale;
}


- (void)scheduleNotificationOn:(NSDate*)fireDate
						  text:(NSString*)text
						action:(NSString*)action
						 sound:(NSString*)soundfileName
				   launchImage:(NSString*)launchImage
					badgeCount:(int)badgeCount
{
	UILocalNotification *localNotification = [[UILocalNotification alloc] init];
    localNotification.fireDate = fireDate;
    localNotification.timeZone = [NSTimeZone defaultTimeZone];	
	
    localNotification.alertBody = text;
    localNotification.alertAction = action;	
	
	if( !soundfileName )
		localNotification.soundName = UILocalNotificationDefaultSoundName;
	else
		localNotification.soundName = soundfileName;
	
	localNotification.alertLaunchImage = launchImage;
    localNotification.applicationIconBadgeNumber = badgeCount;			
	
	// Schedule it with the app
    [[UIApplication sharedApplication] scheduleLocalNotification:localNotification];
    [localNotification release];
}


- (void)cancelAllLocalNotifications
{
	[[UIApplication sharedApplication] cancelAllLocalNotifications];
}


- (void)playMovieAtUrl:(NSString*)url showControls:(BOOL)showControls supportLandscape:(BOOL)landscape supportPortrait:(BOOL)portrait
{
	// are we playing a local file or something on the web?
	BOOL isRemote = [url rangeOfString:@"http"].location == 0;
	
	// Create custom movie player
	P31MoviePlayerViewController *moviePlayer;
	if( isRemote )
	{
		moviePlayer = [[P31MoviePlayerViewController alloc] initWithVideoURL:url];
	}
	else
	{
		// if the url starts with '/' then use it directly else get the bundle path
		if( ![[url substringToIndex:1] isEqualToString:@"/"] )
			url = [[NSBundle mainBundle] pathForResource:url ofType:nil];
		
		if( ![[NSFileManager defaultManager] fileExistsAtPath:url ] )
		{
			NSLog( @"there is no video file at path: %@. aborting video playback", url );
			return;
		}
		
		moviePlayer = [[P31MoviePlayerViewController alloc] initWithVideoFilePath:url];
	}
	
	moviePlayer.delegate = self;
	moviePlayer.supportPortrait = portrait;
	moviePlayer.supportLandscape = landscape;
	
	// Show the movie player modally
	UnityPause( true );
	[UnityGetGLViewController() presentModalViewController:moviePlayer animated:YES];
	
	// Prep and play the movie
	[moviePlayer startPlaybackShowingControls:showControls];
	[moviePlayer release];
}


- (BOOL)addSkipBackupAttributeToItemAtURL:(NSURL*)URL
{
    const char* filePath = [[URL path] fileSystemRepresentation];
    const char* attrName = "com.apple.MobileBackup";
    u_int8_t attrValue = 1;
    int result = setxattr( filePath, attrName, &attrValue, sizeof( attrValue ), 0, 0 );
    return result == 0;
}


@end





// Begin hack until Unity gets the new required notifications in their lifecycle delegate
// this will register for user notifications at app launch automatically. If you are not using local notifications
// you can remove all code below this comment

#import "UnityAppController.h"

void UnitySendDeviceToken( NSData* deviceToken );

@implementation UnityAppController(EtceteraTwoPushAdditions)

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark UIApplicationDelegate

#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 80000

- (void)application:(UIApplication*)application didRegisterUserNotificationSettings:(UIUserNotificationSettings*)notificationSettings
{
	[application registerForRemoteNotifications];
}

#endif


- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken
{
	NSLog( @"didRegisterForRemoteNotificationsWithDeviceToken" );
	UnitySendDeviceToken( deviceToken );
}

@end


