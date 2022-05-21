import 'dart:io';
import 'package:dio/dio.dart';
import 'package:ecommerce_app/main.dart';
import 'package:ecommerce_app/share/cash_helper.dart';
import 'package:ecommerce_app/share/share_api.dart';

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

  Future<Response> post_registNewUser(String email, String password, String phone, String profileName) async
  {
    return await dio.post(
      post_RegisterUser,
      data: {
        "email": email,
        "password": password,
        "phone": phone,
        "profileName": profileName
      }
    );
  }

  Future<Response> post_login(String email, String password) async
  {
    return await dio.post(
      post_LoginUser,
      data: {
        "email": email,
        "password": password
      }
    );
  }

  Future<Response> get_category() async
  {
    return await dio.get(get_GetAllCategories);
  }

  Future<Response> get_item() async
  {
    return await dio.get(get_GetAllItems);
  }

  Future<Response> post_category(String name, String description, String imageUrl) async
  {
    return await dio.post(
      post_CreateaCategory,
      data: {
        "name": name,
        "description": description,
        "imageUrl": imageUrl
      },
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }

  Future<Response> post_item(String name, String description, int price, String imageUrl, int categoryId) async
  {
    return await dio.post(
      post_Item,
      data: {
        "name": name,
        "description": description,
        "price": price,
        "imageUrl": imageUrl,
        "categoryId": categoryId
      },
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }


}