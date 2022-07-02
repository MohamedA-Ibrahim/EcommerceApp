class OrderModel
{
  int? id;
  String? buyerId;
  String? buyerUserName;
  String? buyerProfileName;
  String? buyerPhoneNumber;
  int? itemId;
  String? itemName;
  String? itemDescription;
  int? itemPrice;
  String? itemImageUrl;
  String? shippingDate;
  String? paymentDate;
  String? orderStatus;
  String? paymentStatus;
  String? phoneNumber;
  String? streetAddress;
  String? city;
  String? recieverName;
  String? sellerId;
  String? sellerUserName;
  String? sellerProfileName;
  String? sellerPhoneNumber;

  OrderModel.fromJson(Map<String, dynamic> json)
  {
    id = json["id"];
    buyerId = json["buyer"]["id"];
    buyerUserName = json["buyer"]["userName"];
    buyerProfileName = json["buyer"]["profileName"];
    buyerPhoneNumber = json["buyer"]["phoneNumber"];
    itemId = json["item"]["id"];
    itemName = json["item"]["name"];
    itemDescription = json["item"]["description"];
    itemPrice = json["item"]["price"];
    itemImageUrl = json["item"]["imageUrl"];
    shippingDate = json["shippingDate"];
    paymentDate = json["paymentDate"];
    phoneNumber = json["phoneNumber"];
    paymentStatus = json["paymentStatus"];
    orderStatus = json["orderStatus"];
    streetAddress = json["streetAddress"];
    city = json["city"];
    recieverName = json["recieverName"];
    sellerId = json["item"]["seller"]["id"];
    sellerUserName = json["item"]["seller"]["userName"];
    sellerProfileName = json["item"]["seller"]["profileName"];
    sellerPhoneNumber = json["item"]["seller"]["phoneNumber"];
  }

}