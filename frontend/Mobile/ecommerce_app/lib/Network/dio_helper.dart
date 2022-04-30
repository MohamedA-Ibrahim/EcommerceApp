import 'dart:io';
import 'package:dio/dio.dart';
import 'package:ecommerce_app/main.dart';
import 'package:ecommerce_app/share/cash_helper.dart';

class DioHelper
{
  late Dio dio;

  DioHelper()
  {
    dio = Dio(BaseOptions(
      contentType: Headers.jsonContentType,
      responseType: ResponseType.json,
      validateStatus: (_) => true,

    ));
  }

  Future<Response> post_registNewUser(String email, String password) async
  {
    return await dio.post(
      "https://ecommerceapiservice.azurewebsites.net/api/v1/identity/register",
      data: {
        "email": email,
        "password": password
      }
    );
  }

  Future<Response> post_login(String email, String password) async
  {
    return await dio.post(
      "https://ecommerceapiservice.azurewebsites.net/api/v1/identity/login",
      data: {
        "email": email,
        "password": password
      }
    );
  }

  Future<Response> get_category() async
  {
    return await dio.get("https://ecommerceapiservice.azurewebsites.net/api/v1/categories");
  }

  Future<Response> get_item() async
  {
    return await dio.get("https://ecommerceapiservice.azurewebsites.net/api/v1/items");
  }

  Future<Response> post_category(String name) async
  {
    return await dio.post(
      "https://ecommerceapiservice.azurewebsites.net/api/v1/categories",
      data: {
        "name": name
      },
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }

  Future<Response> post_item(String name, String description, int price, String imageUrl, int categoryId, String expirationDate) async
  {
    return await dio.post(
      "https://ecommerceapiservice.azurewebsites.net/api/v1/items",
      data: {
        "name": name,
        "description": description,
        "price": price,
        "imageUrl": imageUrl,
        "categoryId": categoryId,
        "expirationDate": expirationDate
      },
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }


}