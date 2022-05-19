import 'dart:ffi';

import 'package:ecommerce_app/model/category_model.dart';

class ItemModel
{
  int? id;
  String? name;
  String? description;
  double? price;
  String? imageUrl;
  int? categoryId;
  CategoryModel? category;
  String? expirationDate;

  ItemModel.fromJson(Map<String, dynamic> json)
  {
    id = json["id"];
    name = json["name"];
    description = json["description"];
    price = json["price"].toDouble();
    imageUrl = json["imageUrl"];
    categoryId = json["categoryId"];
    category = CategoryModel.fromJson(json["category"]);
    expirationDate = json["expirationDate"];
  }
}