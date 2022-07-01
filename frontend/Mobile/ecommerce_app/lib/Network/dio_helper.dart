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

  Future<Response> get_attributeType(int categoryId) async
  {
    return await dio.get("$get_AttributeType$categoryId");
  }

  Future<Response> delete_category(int categoryId) async
  {
    return dio.delete("$delete_Category$categoryId",
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }

  Future<Response> getAttributeValues(int itemId) async
  {
    return await dio.get("$get_AttributeValue$itemId");
  }

  Future<Response> getUserAddress() async
  {
    return await dio.get(
      get_UserAddress,
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }

  Future<Response> postUserAddress(String phoneNumber, String streetAddress, String city, String recieverName) async
  {
    return await dio.post(
      post_UserAddress,
      data: {
        "phoneNumber": phoneNumber,
        "streetAddress": streetAddress,
        "city": city,
        "recieverName": recieverName
      },
      options: Options(
        headers:{
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }

  Future<Response> getItemsPostedByUser() async
  {
    return await dio.get(
      get_GetItemsPostedByUser,
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }

  Future<Response> postOrder(int itemId, String sellerId, String phoneNumber, String streetAddress, String city, String recieverName) async
  {
    return await dio.post(
      post_CreateAnOrder,
      data: {
        "itemId": itemId,
        "sellerId": sellerId,
        "phoneNumber": phoneNumber,
        "streetAddress": streetAddress,
        "city": city,
        "recieverName": recieverName
      },
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }

  Future<Response> getBoughtOrders() async
  {
    return await dio.get(
      get_BoughtOrders,
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }

  Future<Response> putUpdateAttributeValueForItem(int id, String value) async
  {
    return await dio.put(
      "$put_AttributeValue$id",
      data: {
        "id": id,
        "value": value
      },
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }

  Future<Response> putUpdateItemDetails(int id, String name, String discription, double price, int categoryId) async
  {
    return await dio.put(
        "$put_UpdateAnItemById$id",
      data: {
        "name": name,
        "description": discription,
        "price": price,
        "categoryId": categoryId,
      },
      options: Options(
          headers: {
            "Authorization": "bearer ${CacheHelper.getToken()}"
          }
      )
    );
  }

  Future<Response> deleteItemById(int id) async
  {
    return await dio.delete(
      "$delete_DeleteAnItemById$id",
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        }
      )
    );
  }
}