using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public event EventHandler OnSaveProgress;
    public event EventHandler OnGetExtraGold;

    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private BannerView bannerView;
    private UnifiedNativeAd unifiedNativeAd;

    private string interstitialAdID;
    private string rewardedAdID;
    private string bannerViewID;
    private string unifiedNativeAdID;

    private bool nativeLoaded = false;


    [SerializeField] GameObject adNativePanel;
    [SerializeField] RawImage adIcon;
    [SerializeField] RawImage adChoices;
    [SerializeField] TextMeshProUGUI adHeadline;
    [SerializeField] TextMeshProUGUI adCallToAction;
    [SerializeField] TextMeshProUGUI adAdvertiser;

    void Start()
    {
        interstitialAdID = "ca-app-pub-7851902488449082/7436385430";
        rewardedAdID = "ca-app-pub-7851902488449082/8390073134";
        bannerViewID = "ca-app-pub-7851902488449082/7215564598";
        unifiedNativeAdID = "ca-app-pub-7851902488449082/9184403762";

        MobileAds.Initialize(initStatus => { });

        RequestInterstitial();
        RequestRewardedVideo();
        //bool bannerAd = UnityEngine.Random.Range(0, 100) < 15;
        //if (bannerAd)
        //    RequestBannerView();
        //RequestUnifiedNativeAd();
    }

    #region Unified Native
    void Update()
    {
        /*if (nativeLoaded)
        {
            nativeLoaded = false;

            Texture2D iconTexture = this.unifiedNativeAd.GetIconTexture();
            Texture2D iconAdChoices = this.unifiedNativeAd.GetAdChoicesLogoTexture();
            string headline = this.unifiedNativeAd.GetHeadlineText();
            string cta = this.unifiedNativeAd.GetCallToActionText();
            string advertiser = this.unifiedNativeAd.GetAdvertiserText();
            adIcon.texture = iconTexture;
            adChoices.texture = iconAdChoices;
            adHeadline.text = headline;
            adAdvertiser.text = advertiser;
            adCallToAction.text = cta;

            //register gameobjects
            unifiedNativeAd.RegisterIconImageGameObject(adIcon.gameObject);
            unifiedNativeAd.RegisterAdChoicesLogoGameObject(adChoices.gameObject);
            unifiedNativeAd.RegisterHeadlineTextGameObject(adHeadline.gameObject);
            unifiedNativeAd.RegisterCallToActionGameObject(adCallToAction.gameObject);
            unifiedNativeAd.RegisterAdvertiserTextGameObject(adAdvertiser.gameObject);

            adNativePanel.SetActive(true); //show ad panel
        }*/
    }

    private void RequestUnifiedNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder(unifiedNativeAdID).ForUnifiedNativeAd().Build();
        adLoader.OnUnifiedNativeAdLoaded += this.HandleOnUnifiedNativeAdLoaded;
        adLoader.LoadAd(AdRequestBuild());
    }

    AdRequest AdRequestBuild()
    {
        return new AdRequest.Builder().Build();
    }

    private void HandleOnUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs args)
    {
        this.unifiedNativeAd = args.nativeAd;
        nativeLoaded = true;
    }
    #endregion

    #region Interstitial
    private void RequestInterstitial()
    {
        interstitialAd = new InterstitialAd(interstitialAdID);
        interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);
    }
    public void ShowInterstitial()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
            RequestInterstitial();
        }

    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }
    #endregion

    #region Rewarded Video
    private void RequestRewardedVideo()
    {
        rewardedAd = new RewardedAd(rewardedAdID);
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }
    public void ShowRewardedVideo()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        RequestRewardedVideo();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardedVideo();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        RequestRewardedVideo();
        OnSaveProgress?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Banner View
    private void RequestBannerView()
    {
        if (bannerView != null)
            bannerView.Destroy();
        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        //bannerView.OnAdLoaded += HandleOnAdLoaded;
        bannerView = new BannerView(bannerViewID, adaptiveSize, AdPosition.Top);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }
    #endregion
}
