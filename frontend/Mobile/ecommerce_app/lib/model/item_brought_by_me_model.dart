import 'package:dio/dio.dart';

class ItemsBroughtByMeModel
{
  List<ItemBroughtByMe> itemsBroughtByMe = [];
  ItemsBroughtByMeModel.fromJson(Response value)
  {
    for(int i = 0; i < value.data.length; i++)
      {
        itemsBroughtByMe.add(ItemBroughtByMe.fromJson(value.data[i]));
      }
  }

}

class ItemBroughtByMe
{
  int? id;
  String? buyer;
  SellerItemModel? seller;
  String? orderDate;
  int? itemId;
  String? itemName;
  String? shippingDate;
  String? paymentDate;
  String? orderStatus;
  String? paymentStatus;
  String? phoneNumber;
  String? streetAddress;
  String? city;
  String? recieverName;

  ItemBroughtByMe.fromJson(Map<String, dynamic> json)
  {
    id = json["id"];
    buyer = json["buyer"];
    seller = SellerItemModel.fromJson(json["seller"]);
    orderDate = json["orderDate"];
    itemId = json["itemId"];
    itemName = json["itemName"];
    shippingDate = json["shippingDate"];
    paymentDate = json["paymentDate"];
    orderStatus = json["orderStatus"];
    paymentStatus = json["paymentStatus"];
    phoneNumber = json["phoneNumber"];
    streetAddress = json["streetAddress"];
    city = json["city"];
    recieverName = json["recieverName"];
  }
}
class SellerItemModel
{
  late String id;
  late String userName;
  late String profileName;
  late String phoneNumber;

  SellerItemModel.fromJson(Map<String, dynamic> json)
  {
    id = json["id"];
    userName = json["userName"];
    profileName = json["profileName"];
    phoneNumber = json["phoneNumber"];
  }
}


// admin
// bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbkBnbWFpbC5jb20iLCJqdGkiOiIxMmYwMjM5MS00ZGQ3LTQxYjMtOWJiYy02NTk4MDg0MTMzNGIiLCJlbWFpbCI6ImFkbWluQGdtYWlsLmNvbSIsInVzZXJJZCI6ImM4NzY0NzQ3LWJlYjktNGIwYS05MjllLTg5ZTBlNDk4N2ZjZSIsInBob25lIjoiMDEwNDU4NzY1NDEiLCJwcm9maWxlTmFtZSI6IlRoZSBBZG1pbiIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTY1NDA3MjA0NSwiZXhwIjoxNjY4NTk0NDQ1LCJpYXQiOjE2NTQwNzIwNDV9.a4TnHw8KQCI3jx2MxFqCj092IX3feajVzBe2hnHikzs

//User
//bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyMUBnbWFpbC5jb20iLCJqdGkiOiJmYjg2Mjc3Mi00MDA5LTQ4MGYtODE1OC1lNmNiODYzN2ZhOTIiLCJlbWFpbCI6InVzZXIxQGdtYWlsLmNvbSIsInVzZXJJZCI6IjAzOTY2MDY2LTM4MDUtNDc5YS05NzAyLTc4MmJmNWEwYWM2MSIsInBob25lIjoiMDEwNDY1NDc4OTEiLCJwcm9maWxlTmFtZSI6InVzZXIgMSIsInJvbGUiOiJVc2VyIiwibmJmIjoxNjU0MDg4Mjc0LCJleHAiOjE2Njg2MTA2NzQsImlhdCI6MTY1NDA4ODI3NH0.sK_YWLTyIQr3XBKS-ymn-LiiPUjIIj-CjLt-zsIfumI