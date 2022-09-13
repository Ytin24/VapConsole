using Newtonsoft.Json;

namespace Alika.Libs.VK.Responses;

public class ConversationResponse
{
    [JsonProperty("conversation")] public ConversationInfo Conversation;
}

public class ConversationInfo
{
    [JsonProperty("in_read")] public int InRead;

    [JsonProperty("last_message_id")] public int LastMessageId;

    [JsonProperty("out_read")] public int OutRead;
    [JsonProperty("peer")] public PeerInfo Peer;

    [JsonProperty("push_settings")] public PeerPushSettings PushSettings;

    [JsonProperty("chat_settings")] public PeerSettings Settings;

    [JsonProperty("unread_count")] public int UnreadCount;

    [JsonProperty("can_write")] public PeerWriteSettings WriteSettings;

    public class PeerInfo
    {
        [JsonProperty("id")] public int Id;

        [JsonProperty("local_id")] public int LocalId;

        [JsonProperty("type")] public string Type;
    }

    public class PeerPushSettings
    {
        [JsonProperty("disabled_forever")] public bool DisabledForever;
        [JsonProperty("disabled_until")] public int DisabledUntil;

        [JsonProperty("no_sound")] public bool NoSound;
    }

    public class PeerWriteSettings
    {
        [JsonProperty("allowed")] public bool Allowed;

        [JsonProperty("reason")] public int Reason;

        /*public string ReasonText()
        {
            Dictionary<int, string> reasons = new Dictionary<int, string> {
            {18, "пользователь заблокирован или удален"},
            {203, "нет доступа к сообществу"},
            {900, "нельзя отправить сообщение пользователю, который в чёрном списке"},
            {901, "пользователь запретил сообщения от сообщества"},
            {902, "пользователь запретил присылать ему сообщения с помощью настроек приватности"},
            {915, "в сообществе отключены сообщения"},
            {916, "в сообществе заблокированы сообщения"},
            {917, "нет доступа к чату"},
            {918, "нет доступа к e-mail"}
        };
            return reasons[this.Reason];
        }*/
    }

    public class PeerSettings
    {
        [JsonProperty("acl")] public AccessSettings Access;

        [JsonProperty("active_ids")] public List<int> ActiveIds;

        [JsonProperty("is_group_channel")] public bool IsChannel;

        [JsonProperty("members_count")] public int MembersCount;
        [JsonProperty("owner_id")] public int OwnerId;

        [JsonProperty("permissions")] public PeerPermissions Permissions;

        [JsonProperty("photo")] public PeerPhotos Photos;


        [JsonProperty("state")] public string State;

        [JsonProperty("title")] public string Title;

        public class PeerPermissions
        {
            [JsonProperty("call")] public string Call;

            [JsonProperty("change_admins")] public string ChangeAdmins;

            [JsonProperty("change_info")] public string ChangeInfo;

            [JsonProperty("change_pin")] public string ChangePin;
            [JsonProperty("invite")] public string Invite;

            [JsonProperty("see_invite_link")] public string SeeInviteLink;

            [JsonProperty("use_mass_mentions")] public string UseMassMentions;
        }

        public class AccessSettings
        {
            [JsonProperty("can_call")] public bool CanCall;
            [JsonProperty("can_change_info")] public bool CanChangeInfo;

            [JsonProperty("can_change_invite_link")]
            public bool CanChangeInviteLink;

            [JsonProperty("can_change_pin")] public bool CanChangePin;

            [JsonProperty("can_change_service_type")]
            public bool CanChangeServiceType;

            [JsonProperty("can_copy_chat")] public bool CanCopyChat;

            [JsonProperty("can_invite")] public bool CanInvite;

            [JsonProperty("can_moderate")] public bool CanModerate;

            [JsonProperty("can_promote_users")] public bool CanPromoteUsers;

            [JsonProperty("can_see_invite_link")] public bool CanSeeInviteLink;

            [JsonProperty("can_use_mass_mentions")]
            public bool CanUseMassMentions;
        }

        public class PeerPhotos
        {
            [JsonProperty("photo_100")] public string Photo100;

            [JsonProperty("photo_200")] public string Photo200;
            [JsonProperty("photo_50")] public string Photo50;
        }
    }
}

public class ConversationMember
{
    [JsonProperty("can_kick")] public bool CanKick;

    [JsonProperty("invited_by")] public int InvitedBy;

    [JsonProperty("is_admin")] public bool IsAdmin;

    [JsonProperty("join_date")] public int JoinDate;
    [JsonProperty("member_id")] public int MemberId;
}

public class ChangeChatPhotoResponse
{
    [JsonProperty("chat")] public MultiDialog Chat;
    [JsonProperty("message_id")] public int MessageId;
}

public class MultiDialog
{
    [JsonProperty("admin_id")] public int AdminId;

    [JsonProperty("id")] public int Id;

    [JsonProperty("is_default_photo")] public bool IsDeaultPhoto;

    [JsonProperty("members_count")] public int MembersCount;

    [JsonProperty("photo_100")] public string Photo100;

    [JsonProperty("photo_200")] public string Photo200;

    [JsonProperty("photo_50")] public string Photo50;

    [JsonProperty("title")] public string Title;
    [JsonProperty("type")] public string Type;

    [JsonProperty("users")] public List<int> Users;
}

public struct GetInviteLinkResponse
{
    [JsonProperty("link")] public string Link;
}