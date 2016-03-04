using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using June;

public class FacebookPictureDownloader {

	public static Dictionary<string, PictureDownloaderJob> m_DownloadsInProgress = new Dictionary<string, PictureDownloaderJob>();

	/// <summary>
	/// Downloads the picture from URL.
	/// </summary>
	/// <param name="avatarURL">Avatar UR.</param>
	/// <param name="callback">Callback.</param>
	public static void DownloadPictureFromURL(string avatarURL, Action<Texture2D> callback) {
		if(string.IsNullOrEmpty(avatarURL))
			return;

		if(Application.internetReachability == NetworkReachability.NotReachable) {
			if(null != callback)
				callback(null);
			
			if(m_DownloadsInProgress.ContainsKey(avatarURL))
				m_DownloadsInProgress.Remove(avatarURL);
			
			return;
		}
		
		// Check if downloading for this id is already in progress
		if(m_DownloadsInProgress.ContainsKey(avatarURL)) {
			// remove old call back with new one
			PictureDownloaderJob picJob = m_DownloadsInProgress[avatarURL];
			picJob.callback = callback;
			return;
		}
		
		// Create a new job and add it to m_DownloadsInProgress list
		PictureDownloaderJob downloadingJob = new PictureDownloaderJob();
		downloadingJob.m_AvatarURL = avatarURL;
		downloadingJob.callback = callback;
		m_DownloadsInProgress.Add(avatarURL, downloadingJob);
		downloadingJob.StartDownload();
	}

	/// <summary>
	/// Gets the local picture.
	/// </summary>
	/// <param name="picName">Pic name.</param>
	/// <param name="callback">Callback.</param>
	public static void GetLocalPicture(string picName, Action<Texture2D> callback) {
		if(string.IsNullOrEmpty(picName))
			return;
		Texture2D texture = Resources.Load(string.Format("UIFaces/{0}", picName)) as Texture2D;
		callback(texture);
	}

	/// <summary>
	/// Downloads the picture.
	/// </summary>
	/// <param name="facebookID">Facebook I.</param>
	/// <param name="callback">Callback.</param>
	public static void DownloadPicture(string facebookID, Action<Texture2D> callback) {
		if(string.IsNullOrEmpty(facebookID))
			return;

		if(Application.internetReachability == NetworkReachability.NotReachable) {
			if(null != callback)
				callback(null);

			if(m_DownloadsInProgress.ContainsKey(facebookID))
				m_DownloadsInProgress.Remove(facebookID);

			return;
		}

		// Check if downloading for this id is already in progress
		if(m_DownloadsInProgress.ContainsKey(facebookID)) {
			// remove old call back with new one
			PictureDownloaderJob picJob = m_DownloadsInProgress[facebookID];
			picJob.callback = callback;
			return;
		}

		// Create a new job and add it to m_DownloadsInProgress list
		PictureDownloaderJob downloadingJob = new PictureDownloaderJob();
		downloadingJob.m_FacebookID = facebookID;
		downloadingJob.callback = callback;
		m_DownloadsInProgress.Add(facebookID, downloadingJob);
		downloadingJob.StartDownload();
	}

	public static bool IsPictureAlreadyDownloaded(string pictureURL) {
		string cachedFileName = Util.SHA1Encode (pictureURL);
		string directoryPath = System.IO.Path.Combine(Application.temporaryCachePath, "fbcache");
		
		if(false == System.IO.Directory.Exists(directoryPath)) {
			System.IO.Directory.CreateDirectory(directoryPath);
		}

		string cachedFilePath = System.IO.Path.Combine(directoryPath, cachedFileName);
		
		//Check if cached image exists
		if (System.IO.File.Exists (cachedFilePath))
			return true;
		else
			return false;
	}

	/// <summary>
	/// Removes the download for facebook I.
	/// </summary>
	/// <param name="facebookID">Facebook I.</param>
	public static void RemoveDownloadForKey(string downloadKey) {
		if(false == string.IsNullOrEmpty(downloadKey) && m_DownloadsInProgress.ContainsKey(downloadKey)) {
			m_DownloadsInProgress.Remove(downloadKey);
		}
	}

	/// <summary>
	/// Removes the callback for facebook I.
	/// </summary>
	/// <param name="facebookID">Facebook I.</param>
	public static void RemoveCallbackForKey(string downloadKey) {
		if(false == string.IsNullOrEmpty(downloadKey) && m_DownloadsInProgress.ContainsKey(downloadKey)) {
			PictureDownloaderJob picJob = m_DownloadsInProgress[downloadKey];
			picJob.callback = null;
		}
	}

	public static void ClearFBCache(string folderName) {
		try {
			LocalStore.Instance.SetInt(LocalStorageKeys.FB_CACHE_CLEARED_TIMESTAMP, Util.CurrentUTCTimestamp);
			// Delete all fbcache
			string directoryPath = System.IO.Path.Combine(Application.temporaryCachePath, folderName);
			if(true == System.IO.Directory.Exists(directoryPath)) {
				System.IO.DirectoryInfo downloadedMessageInfo = new System.IO.DirectoryInfo(directoryPath);
				foreach(System.IO.FileInfo file in downloadedMessageInfo.GetFiles()) { 
					Util.Log("deleting file " + file.Name);
					file.Delete();
				}
			}
		}
		catch {
		}
	}
}

/// <summary>
/// Picture downloader job.
/// </summary>
public class PictureDownloaderJob {
	public string m_FacebookID;
	public string m_AvatarURL;
	public Action<Texture2D> callback;
	private Job m_Job;

	public void StartDownload() {
		m_Job = Job.Create(LoadImage ());
		m_Job.OnJobComplete += OnDownloadComplete;

		// Kill the job if image is not downloaded in 100 seconds
		m_Job.Kill(100);
	}

	void OnDownloadComplete (bool status) {
		if(false == status) {
			callback = null;
			if(false == string.IsNullOrEmpty(m_AvatarURL))
				FacebookPictureDownloader.RemoveDownloadForKey(m_AvatarURL);

			if(false == string.IsNullOrEmpty(m_FacebookID))
				FacebookPictureDownloader.RemoveDownloadForKey(m_FacebookID);
		}
	}

	/// <summary>
	/// Loads the image.
	/// </summary>
	/// <returns>The image.</returns>
	/// <param name="facebookID">Facebook I.</param>
	/// <param name="callback">Callback.</param>
	public IEnumerator LoadImage ()  {
		bool isBogus = false;
		bool cached = false;
		
		//Using SHA for URL as filename 
		string imageUrl = m_AvatarURL;
		if(string.IsNullOrEmpty(imageUrl))
			imageUrl = GetFacebookPictureURLFromID(m_FacebookID);

		string cachedFileName = Util.SHA1Encode (imageUrl);
		string directoryPath = System.IO.Path.Combine(Application.temporaryCachePath, "fbcache");

		if(false == System.IO.Directory.Exists(directoryPath)) {
			System.IO.Directory.CreateDirectory(directoryPath);
		}

		// Delete all fbcache
		/*System.IO.DirectoryInfo downloadedMessageInfo = new System.IO.DirectoryInfo(directoryPath);
		foreach(System.IO.FileInfo file in downloadedMessageInfo.GetFiles()) { 
			Debug.Log("deleting file " + file.Name);
			file.Delete();
		}*/

		string cachedFilePath = System.IO.Path.Combine(directoryPath, cachedFileName);
		
		//Check if cached image exists
		if (System.IO.File.Exists (cachedFilePath)) {
			cached = true;
			//System.IO.File.Delete(string.Format ("/private/{0}", cachedFilePath));
			imageUrl = string.Format ("file://{0}", cachedFilePath);
			Util.Log("[FacebookPictureDownloader] Found Cached Image - " + imageUrl);
		}
		
		//The process of loading the image remains the same, 
		//we just change the URL to point to a local file if found cached
		//Util.Log ("[FacebookPictureDownloader] - Creating www instance.");
		WWW loader = new WWW (imageUrl);
		yield return loader;
		Texture2D texture = null;

		if(null != loader && loader.isDone) //&& string.IsNullOrEmpty(loader.error))
			texture = loader.texture;

		if(null != texture) {
			Util.Log("[FacebookPictureDownloader] - Texture received");
			
			isBogus = IsBogus (loader.texture.EncodeToPNG ());
			
			//Add file to cache if not currently present
			if (false == cached && false == isBogus) {	
				Util.Log("[FacebookPictureDownloader] - Received Image not bogus");
				System.IO.File.WriteAllBytes (cachedFilePath, loader.bytes);
			}
			
			//Delete cached error images
			if (true == cached && true == isBogus) 
			{
				Util.Log("[FacebookPictureDownloader] - Received Image is bogus");
				System.IO.File.Delete (cachedFilePath);
			}
		}

		if (!isBogus && null != callback) 
		{
			callback(texture);
		}

		if(false == string.IsNullOrEmpty(m_AvatarURL))
			FacebookPictureDownloader.RemoveDownloadForKey(m_AvatarURL);

		if(false == string.IsNullOrEmpty(m_FacebookID))
			FacebookPictureDownloader.RemoveDownloadForKey(m_FacebookID);
	}

	/// <summary>
	/// Gets the facebook picture URL from I.
	/// </summary>
	/// <returns>The facebook picture URL from I.</returns>
	/// <param name="facebookID">Facebook I.</param>
	private string GetFacebookPictureURLFromID(string facebookID) {
		if(true == string.IsNullOrEmpty(facebookID)) {
			return string.Empty;
		}
		
		return string.Format("http://graph.facebook.com/{0}/picture", facebookID);
	}

	private bool IsBogus (byte[] imageBytes) {
		if (imageBytes.Length != QuestionMarkPNG.Length) {
			return false;
		}
		for (int i=0; i<QuestionMarkPNG.Length; i++) {
			if (false == QuestionMarkPNG [i].Equals (imageBytes [i])) { 
				return false;
			}
		}
		return true;
	}
	
	private readonly byte[] QuestionMarkPNG = new byte[] {
		137,
		80,
		78,
		71,
		13,
		10,
		26,
		10,
		0,
		0,
		0,
		13,
		73,
		72,
		68,
		82,
		0,
		0,
		0,
		8,
		0,
		0,
		0,
		8,
		8,
		2,
		0,
		0,
		0,
		75,
		109,
		41,
		220,
		0,
		0,
		0,
		65,
		73,
		68,
		65,
		84,
		8,
		29,
		85,
		142,
		81,
		10,
		0,
		48,
		8,
		66,
		107,
		236,
		254,
		87,
		110,
		106,
		35,
		172,
		143,
		74,
		243,
		65,
		89,
		85,
		129,
		202,
		100,
		239,
		146,
		115,
		184,
		183,
		11,
		109,
		33,
		29,
		126,
		114,
		141,
		75,
		213,
		65,
		44,
		131,
		70,
		24,
		97,
		46,
		50,
		34,
		72,
		25,
		39,
		181,
		9,
		251,
		205,
		14,
		10,
		78,
		123,
		43,
		35,
		17,
		17,
		228,
		109,
		164,
		219,
		0,
		0,
		0,
		0,
		73,
		69,
		78,
		68,
		174,
		66,
		96,
		130,
	};

}
