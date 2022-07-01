class AddressModel
{
  late String phoneNumber;
  late String streetAddress;
  late String city;
  late String recieverName;

  AddressModel.fromJson(Map<String, dynamic> json)
  {
    phoneNumber = json["phoneNumber"];
    streetAddress = json["streetAddress"];
    city = json["city"];
    recieverName = json["recieverName"];
  }

}