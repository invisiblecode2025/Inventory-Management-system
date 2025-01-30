
namespace American.Shared.Core.Enum
{
    public class Security
    {
        public enum TokenInfo
        {
            UserId = 1
        }

        public enum ApiAction
        {
            DashBoard = 1,
            SystemManagment = 2,
            Organization = 3,
            Store = 4,
            User = 5,
            Role = 6,
            Application = 7,
            Equipment = 10,
            Device = 11,
            Model = 12,
            Template = 13,
            SystemLogs = 14,
            ToolLog = 15,
            UserLog = 16,
            DeviceEventLogs = 17,
            Reports = 18,
            Support = 19,


            Localization = 20,
        }

        public enum ActionType
        {
            View = 100,
            Add = 101,
            Edit = 102,
            Delete = 103
        }
    }
}
