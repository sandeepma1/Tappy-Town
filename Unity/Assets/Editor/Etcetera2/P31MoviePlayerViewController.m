

#import "P31MoviePlayerViewController.h"

@implementation P31MoviePlayerViewController

@synthesize supportLandscape = _supportLandscape, supportPortrait = _supportPortrait, delegate = _delegate;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

- (id)initWithVideoFilePath:(NSString*)moveFilePath
{
	if( ( self = [super init] ) )
	{
		_supportLandscape = YES;
		NSURL *movieURL = [NSURL fileURLWithPath:moveFilePath];
		_moviePlayer = [[MPMoviePlayerController alloc] initWithContentURL:movieURL];
	}
	return self;
}


- (id)initWithVideoURL:(NSString*)movieUrl
{
	if( ( self = [super init] ) )
	{
		_supportLandscape = YES;
		NSURL *url = [NSURL URLWithString:movieUrl];
		_moviePlayer = [[MPMoviePlayerController alloc] initWithContentURL:url];
	}
	return self;
}


- (void)dealloc
{
	[[NSNotificationCenter defaultCenter] removeObserver:self];
	[_moviePlayer release];
	
	[super dealloc];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark UIViewController

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation
{
	if( _supportLandscape && UIInterfaceOrientationIsLandscape( toInterfaceOrientation ) )
		return YES;
	
	if( _supportPortrait && UIInterfaceOrientationIsPortrait( toInterfaceOrientation ) )
		return YES;

	return NO;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)startPlaybackShowingControls:(BOOL)showControls
{
	_moviePlayer.controlStyle = showControls ? MPMovieControlStyleFullscreen : MPMovieControlStyleNone;
	_moviePlayer.shouldAutoplay = YES;
	[self.view addSubview:_moviePlayer.view];
	_moviePlayer.view.frame = self.view.frame;
	_moviePlayer.view.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
	[_moviePlayer setFullscreen:YES animated:NO];

	// Register to receive a notification when the movie has finished playing.
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(moviePlayBackDidFinish:)
												 name:MPMoviePlayerPlaybackDidFinishNotification
											   object:nil];

	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(moviePlayBackDidFinish:)
												 name:MPMoviePlayerDidExitFullscreenNotification
											   object:nil];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSNotification

- (void)moviePlayBackDidFinish:(NSNotification*)notification
{
	[[UIApplication sharedApplication] setStatusBarHidden:YES];
	
	// Remove the view and observer
	[_moviePlayer.view removeFromSuperview];
	[[NSNotificationCenter defaultCenter] removeObserver:self];
	
	[_delegate moviePlayerControllerDidFinish:self];
}

@end
