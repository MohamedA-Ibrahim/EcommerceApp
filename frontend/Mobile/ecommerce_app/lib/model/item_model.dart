import 'dart:ffi';

import 'package:ecommerce_app/model/category_model.dart';

import '../testing.dart';

class ItemModel
{
  int? id;
  String? name;
  String? description;
  double? price;
  String? imageUrl;
  CategoryModel? category;
  String? expirationDate;
  bool? sold;

  ItemModel.fromJson(Map<String, dynamic> json)
  {
    id = json["id"];
    Log.v("parse id");
    name = json["name"];
    Log.v("parse name");
    description = json["description"];
    Log.v("parse description");
    price = json["price"].toDouble();
    Log.v("parse price");
    imageUrl = json["imageUrl"];
    Log.v("parse imageUIL");
    category = CategoryModel.fromJson(json["category"]);
    Log.v("parse category");
    expirationDate = json["expirationDate"];
    Log.v("parse expiration");
    sold = json["sold"];
    Log.v("parse sold");
  }
}