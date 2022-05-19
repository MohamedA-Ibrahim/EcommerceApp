import '../testing.dart';

class CategoryModel
{
  int? id;
  String? name;
  String? created;
  String? createdBy;
  String? lastModified;
  int? lastModifiedBy;

  CategoryModel.fromJson(Map<String, dynamic> json)
  {
    id = json["id"];
    //Log.v("parse id");
    name = json["name"];
    //Log.v("parse name");
    created = json["created"];
    //Log.v("parse created");
    createdBy = json["createdBy"];
    //Log.v("parse createdby");
    lastModified = json["lastModified"];
    //Log.v("parse lastModified");
    lastModifiedBy = json["lastModifiedBy"];
    //Log.v("parse lastModifiedBy");
  }
}