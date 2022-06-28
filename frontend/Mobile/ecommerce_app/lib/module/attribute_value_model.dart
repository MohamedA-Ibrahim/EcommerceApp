class AttributeValueModel
{
  late int id;
  late String value;
  late int attributeTypeId;
  late String attributeTypeName;

  AttributeValueModel.json(Map<String, dynamic> json)
  {
    id = json["id"];
    value = json["value"];
    attributeTypeId = json["attributeType"]["id"];
    attributeTypeName = json["attributeType"]["name"];
  }
}