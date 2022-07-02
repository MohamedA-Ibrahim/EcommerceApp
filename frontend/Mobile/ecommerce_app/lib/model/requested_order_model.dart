class RequestedOrderModel
{
  int? itemId;
  int? orderId;

  String? buyerUserName;

  String? orderStatus;
  String? paymentStatus;
  String? streetAddress;
  String? city;
  String? phoneNumber;
  String? recieverName;

  RequestedOrderModel.fromJson(Map<String, dynamic> json)
  {
    orderId = json["id"];
    orderStatus = json["orderStatus"];
    paymentStatus = json["paymentStatus"];
    streetAddress = json["streetAddress"];
    city = json["city"];
    phoneNumber = json["phoneNumber"];
    recieverName = json["recieverName"];

    buyerUserName = json["buyer"]["userName"];
    itemId = json["item"]["id"];


  }

}