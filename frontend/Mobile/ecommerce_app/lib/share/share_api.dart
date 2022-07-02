//   http://188.166.59.170:5000/api/v1/
String baseUrl = "https://ecommeapi.azurewebsites.net/api/v1";


String post_RegisterUser = "$baseUrl/identity/register";

String post_LoginUser = "$baseUrl/identity/login";

String post_Image = "$baseUrl/images";

String post_Item = "$baseUrl/items";

String get_GetItemsPostedByUser = "$baseUrl/user/items";

String get_GetAllItems = "$baseUrl/items";

String get_GetAnItemById = "$baseUrl/items/";

String put_UpdateAnItemById = "$baseUrl/items/";

String delete_DeleteAnItemById = "$baseUrl/items/";

String get_GetAllCategories = "$baseUrl/categories";

String post_CreateaCategory = "$baseUrl/categories";

String delete_Category = "$baseUrl/categories/";

String get_GetCategoryById = "$baseUrl/categories/";

String GetUserAddress = "$baseUrl/user/address";

String PostUserAddress = "$baseUrl/user/address";

String post_AttributeType = "$baseUrl/attributeTypes";

String get_AttributeType = "$baseUrl/attributeTypes/";

String post_AttributeValue = "$baseUrl/attributeValues";

String get_AttributeValue = "$baseUrl/attributeValues/";

String put_AttributeValue = "$baseUrl/attributeValues/";

String post_UserAddress = "$baseUrl/user/address";

String get_UserAddress = "$baseUrl/user/address";

String post_CreateAnOrder = "$baseUrl/user/orders";

String get_BoughtOrders = "$baseUrl/user/boughtOrders";

String get_SoldOrders = "$baseUrl/user/soldOrders";

String put_CanselOrder = "$baseUrl/user/orders/cancel/";

String put_RrjrctOrder = "$baseUrl/user/orders/reject/";

String get_ItemDetails = "$baseUrl/items/d/";

String put_StartProcessing = "$baseUrl/user/orders/startProcessing/";

String put_ConfirmPayment = "$baseUrl/user/orders/confirmPayment/";

String put_ShipOrder = "$baseUrl/user/orders/shipOrder/";
