using Newtonsoft.Json;

namespace Alika.Libs.VK.Responses;

public struct Attachment
{
    [JsonProperty("type")] public string Type;

    [JsonProperty("photo")] public PhotoAtt Photo;

    [JsonProperty("video")] public VideoAtt Video;

    [JsonProperty("audio")] public AudioAtt Audio;

    [JsonProperty("doc")] public DocumentAtt Document;

    [JsonProperty("graffiti")] public GraffitiAtt Graffiti;

    [JsonProperty("audio_message")] public AudioMessageAtt AudioMessage;

    [JsonProperty("link")] public LinkAtt Link;

    [JsonProperty("money_transfer")] public MoneyTransferAtt MoneyTransfer;

    [JsonProperty("wall")] public WallAtt Wall;

    [JsonProperty("wall_reply")] public WallReplyAtt WallReply;

    [JsonProperty("sticker")] public StickerAtt Sticker;

    [JsonProperty("gift")] public GiftAtt Gift;

    [JsonProperty("story")] public StoryAtt Story;

    public class PhotoAtt : AttachBase
    {
        [JsonProperty("album_id")] public int AlbumId;

        [JsonProperty("date")] public int Date;

        [JsonProperty("sizes")] public List<Size> Sizes;

        [JsonProperty("text")] public string Text;

        [JsonProperty("user_id")] public int UserId;

        public Size GetBestQuality()
        {
            return Sizes.OrderByDescending(i => i.Width).ThenByDescending(i => i.Height).First();
        }

        public override string ToAttachFormat(bool key = true)
        {
            return "photo" + base.ToAttachFormat(key);
        }

        public class Size
        {
            [JsonProperty("height")] public int Height;
            [JsonProperty("type")] public string Type;

            [JsonProperty("url")] public string Url;

            [JsonProperty("width")] public int Width;
        }
    }

    public class VideoAtt : AttachBase
    {
        [JsonProperty("adding_date")] public int AddingDate;

        [JsonProperty("can_add")] public int CanAdd;

        [JsonProperty("can_edit")] public int CanEdit;

        [JsonProperty("comments")] public int Comments;

        [JsonProperty("date")] public int Date;

        [JsonProperty("description")] public string Description;

        [JsonProperty("duration")] public int Duration;

        [JsonProperty("first_frame_1280")] public string FirstFrame1280;

        [JsonProperty("first_frame_130")] public string FirstFrame130;

        [JsonProperty("first_frame_320")] public string FirstFrame320;

        [JsonProperty("first_frame_640")] public string FirstFrame640;

        [JsonProperty("first_frame_800")] public string FirstFrame800;

        [JsonProperty("is_favorite")] public bool IsFavorite;

        [JsonProperty("is_private")] public int IsPrivate;

        [JsonProperty("live")] public int Live;

        [JsonProperty("photo_1280")] public string Photo1280;

        [JsonProperty("photo_130")] public string Photo130;

        [JsonProperty("photo_320")] public string Photo320;

        [JsonProperty("photo_640")] public string Photo640;

        [JsonProperty("photo_800")] public string Photo800;

        [JsonProperty("platform")] public string Platform;

        [JsonProperty("player")] public string Player;

        [JsonProperty("processing")] public int Processing;
        [JsonProperty("title")] public string Title;

        [JsonProperty("upcoming")] public int Upcoming;

        [JsonProperty("views")] public int Views;

        public override string ToAttachFormat(bool key = true)
        {
            return "video" + base.ToAttachFormat(key);
        }
    }

    public class AudioAtt : AttachBase
    {
        [JsonProperty("album_id")] public int AlbumId;
        [JsonProperty("artist")] public string Artist;

        [JsonProperty("date")] public int Date;

        [JsonProperty("duration")] public int Duration;

        [JsonProperty("genre_id")] public int GenreId;

        [JsonProperty("is_hq")] public int IsHQ;

        [JsonProperty("lyrics_id")] public int LyricsId;

        [JsonProperty("no_search")] public int NoSearch;

        [JsonProperty("title")] public string Title;

        [JsonProperty("url")] public string Url;

        public override string ToAttachFormat(bool key = true)
        {
            return "audio" + base.ToAttachFormat(key);
        }
    }

    public class DocumentAtt : AttachBase
    {
        [JsonProperty("date")] public string Date;

        [JsonProperty("ext")] public string Extension;

        [JsonProperty("preview")] public DocPreview Preview;

        [JsonProperty("size")] public int Size;
        [JsonProperty("title")] public string Title;

        [JsonProperty("type")] public string Type;

        [JsonProperty("url")] public string Url;

        public override string ToAttachFormat(bool key = true)
        {
            return "doc" + base.ToAttachFormat(key);
        }

        public class DocPreview
        {
            [JsonProperty("audio_message")] public AudioMessageAtt AudioMessage;

            [JsonProperty("graffiti")] public GraffitiAtt Graffiti;
            [JsonProperty("photo")] public PhotoAtt Photo;
        }
    }

    public class GraffitiAtt : AttachBase
    {
        [JsonProperty("height")] public int Height;
        [JsonProperty("url")] public string Url;

        [JsonProperty("width")] public int Width;

        public override string ToAttachFormat(bool key = true)
        {
            return "doc" + base.ToAttachFormat(key);
        }
    }

    public class StoryAtt : AttachBase
    {
        public override string ToAttachFormat(bool key = true)
        {
            return "story" + base.ToAttachFormat(key);
        }
    }

    public class AudioMessageAtt : AttachBase
    {
        [JsonProperty("duration")] public int Duration;

        [JsonProperty("link_mp3")] public string LinkMP3;

        [JsonProperty("link_ogg")] public string LinkOGG;

        [JsonProperty("transcript")] public string Transcript;

        [JsonProperty("transcript_state")] public string TranscriptState;

        [JsonProperty("waveform")] public List<int> Waveform;

        public override string ToAttachFormat(bool key = true)
        {
            return "doc" + base.ToAttachFormat(key);
        }
    }

    public class LinkAtt
    {
        [JsonProperty("caption")] public string Caption;

        [JsonProperty("description")] public string Description;

        [JsonProperty("photo")] public PhotoAtt Photo;

        [JsonProperty("title")] public string Title;
        [JsonProperty("url")] public string Url;
    }

    public class WallAtt
    {
        [JsonProperty("date")] public int Date;

        [JsonProperty("friends_only")] public int FriendsOnly;

        [JsonProperty("from_id")] public int FromId;
        [JsonProperty("id")] public int Id;

        [JsonProperty("owner_id")] public int OwnerId;

        [JsonProperty("reply_owner_id")] public int ReplyOwnerId;

        [JsonProperty("reply_post_id")] public int ReplyPostId;

        [JsonProperty("text")] public string Text;

        [JsonProperty("to_id")] public int ToId;
    }

    public class WallReplyAtt
    {
        [JsonProperty("date")] public int Date;

        [JsonProperty("from_id")] public int FromId;
        [JsonProperty("id")] public int Id;

        [JsonProperty("owner_id")] public int OwnerId;

        [JsonProperty("post_id")] public int PostId;

        [JsonProperty("reply_to_comment")] public int ReplyToComment;

        [JsonProperty("reply_to_user")] public int ReplyToUser;

        [JsonProperty("text")] public string Text;
    }

    public class StickerAtt
    {
        [JsonProperty("animation_url")] public string AnimationUrl;

        [JsonProperty("images")] public List<Image> Images;

        [JsonProperty("images_with_background")]
        public List<Image> ImagesWithBackground;

        [JsonProperty("is_allowed")] public bool IsAllowed;
        [JsonProperty("product_id")] public int ProductId;

        [JsonProperty("sticker_id")] public int StickerId;

        public string GetBestQuality(bool background = false)
        {
            return (background ? ImagesWithBackground : Images).OrderByDescending(i => i.Width)
                .ThenByDescending(i => i.Height).First().Url;
        }

        public class Image
        {
            [JsonProperty("height")] public int Height;
            [JsonProperty("url")] public string Url;

            [JsonProperty("width")] public int Width;
        }
    }

    public class GiftAtt
    {
        [JsonProperty("id")] public int Id;

        [JsonProperty("thumb_256")] public string Thumb256;

        [JsonProperty("thumb_48")] public string Thumb48;

        [JsonProperty("thumb_96")] public string Thumb96;
    }

    public class MoneyTransferAtt
    {
        [JsonProperty("amount")] public AmountInfo Amount;

        [JsonProperty("by_phone")] public bool ByPhone;

        [JsonProperty("comment")] public string Comment;

        [JsonProperty("date")] public int Date;

        [JsonProperty("from_id")] public int FromId;
        [JsonProperty("id")] public int Id;

        [JsonProperty("is_anonymous")] public bool IsAnonymous;

        [JsonProperty("is_vkpay")] public bool IsVKPay;

        [JsonProperty("status")] public int Status;

        [JsonProperty("to_id")] public int ToId;

        public struct AmountInfo
        {
            [JsonProperty("amount")] public string Amount;

            [JsonProperty("currency")] public CurrencyInfo Currency;

            [JsonProperty("text")] public string Text;

            public struct CurrencyInfo
            {
                [JsonProperty("id")] public int Id;

                [JsonProperty("name")] public string Name;
            }
        }
    }

    public abstract class AttachBase
    {
        [JsonProperty("access_key")] public string AccessKey;
        [JsonProperty("id")] public int Id;

        [JsonProperty("owner_id")] public int OwnerId;

        public virtual string ToAttachFormat(bool key = true)
        {
            return OwnerId + "_" + Id + (AccessKey?.Length > 0 && key ? "_" + AccessKey : "");
        }
    }
}

public class GetHistoryAttachmentsResponse : ItemsResponse<GetHistoryAttachmentsResponse.AttachmentElement>
{
    [JsonProperty("next_from")] public string NextFrom;

    public class AttachmentElement
    {
        [JsonProperty("attachment")] public Attachment Attachment;

        [JsonProperty("from_id")] public int FromId;
        [JsonProperty("message_id")] public int MessageId;
    }
}