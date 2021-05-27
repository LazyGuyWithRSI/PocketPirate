using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodenGUI
{
    public class LeadersMenu : Menu
    {
        public Button BackButton;
        public Toggle DailyTab;
        public Toggle FriendsTab;
        public Toggle AllTimeTab;
        public Toggle MonthlyTab;
        public ScrollRect ScrollRect;
        public GameObject Prefab;
        public Transform Content;
        public Button GoUpButton;
        public Button GoDownButton;

        public List<Sprite> Avatars;
        private const int AvatarMaxRange = 16;

        public LeaderboardCellView FirstLeaderboardCellView;
        public LeaderboardCellView SecondLeaderboardCellView;
        public LeaderboardCellView ThirdLeaderboardCellView;

        private List<GameObject> _leaderboardCells = new List<GameObject>();
        
        private List<string> _names = new List<string>
        {
            "Anthony",
            "Marcus",
            "Nina",
            "Alex",
            "Timothy",
            "Jacob",
            "Amanda",
            "Samantha",
            "Oliver"
        };
        private System.Random _random = new System.Random();

        private List<LeaderboardData> _daily;
        private List<LeaderboardData> _monthly;
        private List<LeaderboardData> _friends;
        private List<LeaderboardData> _allTime;
        
        private void Start()
        {
            ScrollRect.onValueChanged.AddListener(OnScrollPositionChanged);
            GoUpButton.onClick.AddListener(ScrollToTop);
            GoDownButton.onClick.AddListener(ScrollToBottom);
            BackButton.onClick.AddListener(OnBackButton);
            DailyTab.onValueChanged.AddListener(OnDailyToggle);
            MonthlyTab.onValueChanged.AddListener(OnMonthlyToggle);
            AllTimeTab.onValueChanged.AddListener(OnAllTimeToggle);
            FriendsTab.onValueChanged.AddListener(OnFriendsToggle);
            OnScrollPositionChanged(Vector2.up);

            _daily = GetDataList();
            _monthly = GetDataList();
            _friends = GetDataList();
            _allTime = GetDataList();

            OnDailyToggle(true);
        }

        private void OnDailyToggle(bool enable)
        {
            if(enable)
                PopulateList(_daily);
        }

        private void OnMonthlyToggle(bool enable)
        {
            if (enable)
                PopulateList(_monthly);
        }

        private void OnAllTimeToggle(bool enable)
        {
            if (enable)
                PopulateList(_allTime);
        }

        private void OnFriendsToggle(bool enable)
        {
            if (enable)
                PopulateList(_friends);
        }
        private void OnBackButton()
        {
            UIManager.Instance.GoBack();
        }

        private void PopulateList(List<LeaderboardData> list)
        {
            foreach (var cell in _leaderboardCells)
            {
                Destroy(cell);
            }
            FirstLeaderboardCellView.Image.sprite = Avatars[list[0].avatarId];
            FirstLeaderboardCellView.Name.text = list[0].name;
            
            SecondLeaderboardCellView.Image.sprite = Avatars[list[1].avatarId];
            SecondLeaderboardCellView.Name.text = list[1].name;
            
            ThirdLeaderboardCellView.Image.sprite = Avatars[list[2].avatarId];
            ThirdLeaderboardCellView.Name.text = list[2].name;
            
            for (int i = 3; i < list.Count; i++)
            {
                var data = list[i];
                var go = GameObject.Instantiate(Prefab, Content);
                var cellView = go.GetComponent<LeaderboardCellView>();
                cellView.Image.sprite = Avatars[data.avatarId];
                cellView.Name.text = data.name;
                _leaderboardCells.Add(go);
            }
        }

        private void OnScrollPositionChanged(Vector2 position)
        {
            GoUpButton.gameObject.SetActive(false);
            GoDownButton.gameObject.SetActive(false);
            if (position.y < 0.1f)
            {
                GoUpButton.gameObject.SetActive(true);
            }
            else if(position.y > 0.9f)
            {
                GoDownButton.gameObject.SetActive(true);
            }
        }
        
        public void ScrollToTop()
        {
            ScrollRect.normalizedPosition = new Vector2(0, 1);
        }
        public void ScrollToBottom()
        {
            ScrollRect.normalizedPosition = new Vector2(0, 0);
        }
        private void OnDestroy()
        {
            DailyTab.onValueChanged.RemoveAllListeners();
            FriendsTab.onValueChanged.RemoveAllListeners();
            MonthlyTab.onValueChanged.RemoveAllListeners();
            AllTimeTab.onValueChanged.RemoveAllListeners();
            BackButton.onClick.RemoveAllListeners();
        }

        private List<LeaderboardData> GetDataList()
        {
            var list = new List<LeaderboardData>();

            for (int i = 0; i < 10; i++)
            {
                var avatarId = _random.Next(0, AvatarMaxRange);
                int index = _random.Next(_names.Count);

                list.Add(new LeaderboardData
                {
                    name = _names[index],
                    avatarId = avatarId
                });
            }
            return list;
        }
    }

    public class LeaderboardData
    {
        public string name;
        public int avatarId;
    }
}