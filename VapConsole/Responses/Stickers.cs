using Newtonsoft.Json;

namespace Alika.Libs.VK.Responses;

public class StickerPackInfo
{
    [JsonProperty("author")] public string Author;

    [JsonProperty("background")] public string Background;

    [JsonProperty("can_purchase")] public int CanPurchase;

    [JsonProperty("description")] public string Description;

    [JsonProperty("new")] public int IsNew;

    [JsonProperty("payment_type")] public string PaymentType;

    [JsonProperty("price")] public int Price;

    [JsonProperty("price_buy")] public int PriceBuy;
    [JsonProperty("product")] public ProductInfo Product;

    public class ProductInfo
    {
        [JsonProperty("active")] public int Active;

        [JsonProperty("base_id")] public int BaseId;

        [JsonProperty("icon")] public IconsInfo Icons;
        [JsonProperty("id")] public int Id;

        [JsonProperty("previews")] public List<Attachment.PhotoAtt.Size> Previes;

        [JsonProperty("purchased")] public int Purchased;

        [JsonProperty("stickers")] public List<Attachment.StickerAtt> Stickers;

        [JsonProperty("style_ids")] public List<int> StyleIds;

        [JsonProperty("title")] public string Title;

        [JsonProperty("type")] public string Type;

        [JsonProperty("url")] public string Url { get; set; }

        public struct IconsInfo
        {
            [JsonProperty("base_url")] public string BaseUrl;
        }
    }
}

public class GetStickersKeywordsResponse
{
    [JsonProperty("count")] public int Count;

    [JsonProperty("dictionary")] public List<DictionaryInfo> Dictionary;

    public class DictionaryInfo
    {
        [JsonProperty("promoted_stickers")] public List<Attachment.StickerAtt> promoted_stickers;

        [JsonProperty("user_stickers")] public List<Attachment.StickerAtt> UserStickers;
        [JsonProperty("words")] public List<string> Words;
    }
}