import '../testing.dart';

class CategoryModel
{
  int? id;
  String? name;
  String? createdBy;
  String? description;
  String? imageUrl;

  CategoryModel.fromJson(Map<String, dynamic> json)
  {
    id = json["id"];
    Log.v("parse id");
    name = json["name"];
    Log.v("parse name");
    description = json["description"];
    Log.v("parse created");
    createdBy = json["createdBy"];
    Log.v("parse createdby");
    imageUrl = json["imageUrl"];
    Log.v("parse imageUrl");
  }
}