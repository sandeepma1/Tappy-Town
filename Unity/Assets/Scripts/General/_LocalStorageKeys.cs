using UnityEngine;
using System.Collections;

public class LocalStorageKeys {

	#region Player Profile Keys
	public const string PLAYER_ID = "player_id";
	public const string PLAYER_NAME = "player_name";
	public const string FULL_NAME = "full_name";
	public const string PLAYER_XP = "XP";

	public const string COINS_KEY = "coins";
	public const string LADOOS_KEY = "ladoos";

	public const string MYSTERYBOX_KEY = "mysterybox";
	public const string CURRENT_OUTFIT = "current_outfit";

	public const string HAT_KEY = "hat";
	public const string BAT_KEY = "bat";
	public const string STAR_KEY = "star";

	public const string SOAP_KEY = "soap";
	public const string TOOTHPASTE_KEY = "toothpaste";
	public const string BISCUIT_KEY = "biscuit";
	public const string CAKE_KEY = "cake";
	public const string DONUT_KEY = "donut";
	public const string CHOC_KEY = "choc";
	public const string MILK_KEY = "milk";
	public const string SANDWICH_KEY = "sandwich";


	public const string COINS_SPENT_KEY = "coins_spent";
	public const string LADOOS_SPENT_KEY = "ladoos_spent";
	public const string MYSTERY_BOX_OPENED_KEY = "mysterybox_opened";


	public const string OFFER_SEQUENCE = "offer_sequence";

	public const string MONEY_SPENT_KEY = "money_spent";
	public const string INAPP_PURCHASE_COUNT_KEY = "inapp_purchase_count";
	public const string CURRENT_CHARACTER_KEY = "current_character";
	public const string IS_FIRST_TIME_KEY = "is_first_time";
	public const string IS_FRIENDS_FIRST_TIME_KEY = "is_friends_first_time";
	public const string IS_ADD_FRIENDS_FIRST_TIME_KEY = "is_add_friends_first_time";
	public const string IS_RATED_APP_KEY = "is_rated_app";
	public const string LAST_PLAYED_TIME_KEY = "last_played_time";
	public const string LAST_REFILL_TIME_KEY = "last_refill_time";
	public const string LOCAL_STORAGE_UPGRADE_KEY = "is_updated";
	public const string CURRENT_VERSION_CODE_KEY = "current_app_version";
	public const string TROPHIES_KEY = "current_trophies";
	public const string INSTALLED_DATE = "installed_date";
	public const string IS_TUTORIAL_RACE_PLAYED = "is_tutorial_race_played";
	public const string XP_KEY = "current_xp";
	public const string MEDAL_KEY = "current_medals";
	public const string DONT_SHOW_STORE_BADGE = "dont_show_store_badge";
	public const string IS_RAIN_SELECTED = "is_rain_selected";
	public const string SWIM_TUTORIAL_SHOWN_COUNT = "swim_tutorial_shown";
	public const string TOTAL_FRIENDS_COUNT = "TOTAL_FRIEND_COUNT";
	public const string TOTAL_SPIN_COUNT = "TOTAL_SPIN_COUNT";
	public const string IS_PLAYED_RAIN = "is_player_rain";

	public const string TAUNTS_OWNED_KEY = "taunts_owned";
	public const string CARDS_UNLOCKED = "cards_unlocked";
	public const string UNLOCKED_ITEMS = "unlocked_items";
	public const string SKIPPED_MISSIONS = "skipped_missions";
	public const string COMPLETED_MISSIONS = "completed_missions";


	public const string START_SESSION_CONFLICT_CHECK = "start_session_conflict_check";
	public const string NAME_CHANGE_TUTORIAL_SHOWN = "NameChangeTutorialShown";

	public const string FIRST_DOLLAR_PURCHASE_TYPE = "first_dollar_purchase_type";
	public const string APP_IS_INSTALLED = "app_is_installed";

	public const string IS_FB_MISMATCH = "is_fb_mismatch";
	public const string IS_GC_MISMATCH = "is_gc_mismatch";
	public const string MISMATCH_NAME = "mismatch_name";

	public const string IS_MULTIPLAYER_UNLOCK_SHOWN = "is_multiplayer_unlock_shown";

	public const string IS_CHARACTER_PURCHASED_FORMAT = "purchased_{0}";
	public const string LAST_SELECTED_CHARACTER_COLOUR_FORMAT = "last_colour_{0}";
	public const string SHOW_RATE_US_POPUP = "show_rate_us_popup";
	public const string RATE_US_POPUP_SHOWN_FOR_LEVEL = "rate_us_popup_shown_for_{0}";
	public const string HAS_RATED_THE_APP = "has_rated_app";

	public const string DEVICE_SETTING_FORMAT = "DevSetting_{0}";
	public const string FB_CACHE_CLEARED_TIMESTAMP = "fb_cache_cleared_timestamp";
	public const string FB_CONNECT_POPUP_SHOWN_LEVEL = "fb_connect_shown_level";
	public const string FB_CONNECT_POPUP_SHOWN_COUNT = "fb_connect_shown_count";

	public const string LAST_APP_VERSION = "last_app_version";
	public const string FRIENDS_CENTER_OPENED = "friends_center_opened";
	public const string AMAZON_USER_ID = "Amazon_userId";

	public const string LEAGUE_JADE_COUNT = "LJ";
	public const string LEAGUE_COIN_COUNT = "LC";
	public const string VIDEO_COIN_COUNT = "VC";
	public const string VIDEO_SHOWN_COUNT = "video_shown_count";

	#endregion

	#region Special Packs

	public const string COIN_DOUBLER_PURCHASED = "coin_doubler_purchased";
	public const string REMOVE_ADS_PURCHASED = "remove_ads_purchased";

	#endregion

	#region Game setting Keys
	public const string IS_BG_MUSIC_ON = "IS_BG_MUSIC_ON";
	public const string IS_SOUND_FX_ON = "IS_SOUND_FX_ON";
	public const string IS_DETAILED_STATS_ENABLED = "detailed_stats";
	public const string LAST_PLAYED_LEVEL = "Last_Played_Level";
	public const string LAST_PLAYED_WEATHER = "Last_Played_Weather";
	public const string LAST_PLAYED_WEATHER_COUNT = "Last_Played_Weather_Count";
	#endregion

	#region Cumulative Game Stats
	public static readonly string LEVEL_FORMAT = "{0}_";
	public const string TOTAL_RACES = "total_races";
	public const string TOTAL_RACES_WON = "total_races_won";
	public const string TUTORIAL_INIT_COUNT = "tutorial_initialized_count";
	public const string TUTORIAL_PAUSED_COUNT = "tutorial_paused_count";
	public const string TUTORIAL_QUIT_COUNT = "tutorial_quit_count";
	public const string TUTORIAL_RESUMED_COUNT = "tutorial_resumed_count";
	public const string TUTORIAL_COMPLETED_COUNT = "tutorial_completed_count";
	public const string TOTAL_DOLLAR_PURCHASES_MADE = "total_dollar_purchases_made";	
	public const string TOTAL_DOLLARS_SPENT = "total_dollars_spent";
	public const string TOTAL_COINS_SPENT = "total_coins_spent";
	public const string TOTAL_JADE_SPENT = "total_jade_spent";
	public const string TOTAL_DAILY_BONUS_COLLECTED_COUNT = "total_daily_bonus_collected";
	
	public const string TUTORIAL_CONTROLS_POPUP_SHOWN_COUNT = "tutorial_controls_popup_shown_count";
	public const string TUTORIAL_CONTROLS_POPUP_CLOSED_COUNT = "tutorial_controls_popup_closed_count";
	public const string TUTORIAL_CONTROLS_POPUP_NEXT_TAPPED_COUNT = "tutorial_controls_popup_next_tapped_count";
	public const string TUTORIAL_PAUSE_RETRIED_COUNT = "tutorial_pause_retried_count";
	public const string TUTORIAL_PAUSE_RESUMED_COUNT = "tutorial_pause_resumed_count";
	public const string TUTORIAL_POWERS_POPUP_SHOWN_COUNT = "tutorial_powers_popup_shown_count";
	public const string TUTORIAL_POWERS_POPUP_CLOSED_COUNT = "tutorial_powers_popup_closed_count";
	public const string TUTORIAL_POWERS_POPUP_NEXT_TAPPED_COUNT = "tutorial_powers_popup_next_tapped_count";
	public const string TUTORIAL_QUICKRACE1_INIT_COUNT = "tutorial_quickrace1_init_count";
	public const string TUTORIAL_QUICKRACE1_FINISHED_COUNT = "tutorial_quickrace1_finished_count";
	public const string TUTORIAL_NEXTRACE2_TAPPED_COUNT = "tutorial_nextrace2_tapped_cont";
	public const string TUTORIAL_QUICKRACE2_INIT_COUNT = "tutorial_quickrace2_init_count";
	public const string TUTORIAL_QUICKRACE2_FINISHED_COUNT = "tutorial_quickrace2_finished_count";	
	
	public const string REGISTER_DONE_TAPPED_COUNT = "register_done_tapped_count";
	public const string REGISTER_NAME_CONFIRM_POPUP_SHOWN_COUNT = "register_name_confirm_popup_shown_count";
	public const string REGISTER_NAME_POPUP_OK_TAPPED_COUNT = "register_name_popup_ok_tapped_count";
	public const string REGISTER_NAME_POPUP_CANCELLED_COUNT = "register_name_popup_cancelled_count";
	
	#endregion

	public const string FACEBOOK_USER_ID = "facebook_user_id";
	public const string FACEBOOK_ACCESS_TOKEN = "facebook_access_token";
	public const string FACEBOOK_WAIT_FOR_CALLBACK = "facebook_callback";

	public const string GAME_CENETER_IGNORE = "game_center_ignore";
	public const string GAME_CENTER_ID = "game_center_id";
	public const string GAMECENTER_WAIT_FOR_CALLBACK = "gamecenter_callback";

	public const string APNS_TOKEN = "apns_token";
	public const string GCM_TOKEN = "gcm_token";
	public const string ADP_TOKEN = "adp_token";
	
	public const string LAST_SYNC_TIME = "last_sync_time";
	public const string LAST_SYNC_STATUS = "last_sync_status";
	
	public const string IS_AD_DISABLED = "ads_disabled";
	public const string AD_PROVIDER = "ad_provider";
	public const string AD_FREQUENCY = "ad_frequency";
	public const string AD_DISPLAY_COUNTER = "ad_display_counter";
	public const string LAST_AD_ID = "last_ad_id";
    
	public const string IS_RATED = "is_rated";
    public const string RATE_US_FREQUENCY = "rate_us_frequency";
    
	public const string LAST_BACKUP_TIME = "last_backup_time";
	
	public const string FACEBOOK_FAILED_COUNT = "facebook_failed";
	public const string OUTFIT_PURCHASE_CANCEL_COUNT = "outfit_purchase_cancel";
	public const string GEMS_PURCHASE_CANCEL_COUNT = "gems_purchase_cancel";

	public const string SERVER_UTC_TIME = "server_utc_time";
	
	public const string HASH_KEY = "hash_key";
	public const string USER_REGISTRATION_STEP_KEY = "registration_step_key";
	public const string HAS_SHOWN_INAPP_MESSAGE = "has_shown_inapp";

	#region OFFERS
	public const string OFFER_FB_LOGIN = "offer_fb_login";
	public const string OFFER_FB_INVITE = "offer_fb_invite";
	public const string OFFER_SEEN_ARRAY = "offer_seen_array";
	public const string HOME_OFFER_SEEN_ARRAY = "sof";
	public const string HOME_OFFER = "home_offer";
	public const string HOME_OFFER_PURCHASED = "home_offer_purchased";
	public const string HOME_OFFER_POPUP_SHOWN = "home_offer_popup_shown";
	#endregion

	public const string CARDS_OWNED = "cards";
	public const string UNOPENED_CARD_PACKS = "unopened_card_packs";
	
	public const string UPDATE_NO = "update_no";
	public const string UPDATE_NOTES_SHOWN = "update_notes_shown";

	public const string LEAGUE_NOTIFICATION = "league_notif";

#if UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_EDITOR
	public const string PUSH_NOTIFICATION_TOKEN = APNS_TOKEN;
#elif UNITY_ANDROID
	public const string PUSH_NOTIFICATION_TOKEN = GCM_TOKEN;
#endif
	
	#region Migration
	public static readonly string[] MIGRATION_KEYS_TO_IGNORE = {
		FACEBOOK_USER_ID,
		FACEBOOK_ACCESS_TOKEN,
		PUSH_NOTIFICATION_TOKEN
	};
	#endregion

	#region ImportantKeys
	public static readonly string[] IMPORTANT_KEYS = {
		CURRENT_CHARACTER_KEY,TROPHIES_KEY,COINS_KEY,PLAYER_NAME,XP_KEY,LOCAL_STORAGE_UPGRADE_KEY
	};
	#endregion

	#region Keys To Ignore on Logout
	public static readonly string[] LOGOUT_KEYS_TO_IGNORE = {
		GAME_CENETER_IGNORE
	};
	#endregion

    public static readonly string GOOGLE_PLAY_ID = "google_play_id";
	public static readonly string IS_KEY_CHAIN_ACCESSIBLE = "keychain";

    public static readonly string DATA_VERSION = "data_ver";
    public static readonly string DATA_VERSION_GAME_ELEMENT = "data_ver_ge";
    public static readonly string DATA_VERSION_PLAYER_LEVEL = "data_ver_xp";

	public static readonly string TOTAL_HITS = "total_hits";
	public static readonly string TOTAL_DEATHS = "total_deaths";
	public static readonly string LOOSE_STREAK = "loose_streak";
	public static readonly string WIN_STREAK = "win_streak";
	public static readonly string FIRST_WIN = "first_win";

	public static readonly string LEADERBOARD_SYNC_DATA = "leaderboard_sync";

	public const string IS_ENGLISH = "is_english";

	public const string CTA_BEFORE_PURCHASE_COUNT_FORMAT = "{0}_cta_bfr";
	public const string CTA_CAN_PURCHASE_COUNT_FORMAT = "{0}_cta_pur";
	
	public const string CURRENT_MISSION_DATA = "curr_miss_data";
	public const string CURRENT_SCRIPTMISSION_DATA = "curr_scrmiss_data";
	public const string CURRENT_MISSIONSET_ID = "curr_miss_set_id";

	public const string CURRENT_MISSION_ID = "curr_stationset_id";
	public const string CURRENT_STATION_ID = "curr_station_id";

	public const string CURRENT_MISSION_COMPLETION = "curr_miss_comp";
	public const string CURRENT_MISSION_UPDATE_TEXT = "curr_miss_ut";
	public const string MISSIONS_COMPLETED_COUNT = "missions_completed_count";
	public const string FIRST_MISSION_COMPLETED = "first_mission_completed";
	public const string RACES_PLAYED_CURRENT_MISSION = "races_played_current_mission";
	public const string GOT_EVERYTHING = "got_everything";
	public const string GOT_ALL_OUTFITS = "got_all_outfits";

	public const string SPIN_TYPE_COUNT_FORMAT = "SPIN_{0}_COUNT";

	public const string PREVIOUS_ITEM_ID = "previous_item";

	public const string RENTED_ITEM_ID = "rented_id";
	public const string RENTED_ITEM_EXPIRY = "rented_expiry";
	public const string RENTED_ITEM_VALUE = "rented_value";

	public const string EMAIL = "email";

	public const string DAILY_REWARD_START = "dr_start";
	public const string DAILY_REWARD_LAST = "dr_last";

	public const string RENT_ITEM_SHOWN = "rshown";
	public const string RENT_ITEM_SHOWN_COUNT = "rshowncnt";

}
