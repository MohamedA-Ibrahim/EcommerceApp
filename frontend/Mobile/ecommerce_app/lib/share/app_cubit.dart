import 'dart:convert';
import 'dart:io';
import 'package:dio/dio.dart';
import 'package:ecommerce_app/Network/dio_helper.dart';
import 'package:ecommerce_app/layout/home_screen.dart';
import 'package:ecommerce_app/main.dart';
import 'package:ecommerce_app/model/category_model.dart';
import 'package:ecommerce_app/model/item_model.dart';
import 'package:ecommerce_app/model/user_model.dart';
import 'package:ecommerce_app/module/items_module.dart';
import 'package:ecommerce_app/module/user_module.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:ecommerce_app/share/cash_helper.dart';
import 'package:ecommerce_app/share/share_api.dart';
import 'package:ecommerce_app/testing.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_facebook_auth/flutter_facebook_auth.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:permission_handler/permission_handler.dart';

import '../model/address_model.dart';
import '../model/item_brought_by_me_model.dart';
import '../module/categoru_module.dart';

class AppCubit extends Cubit<AppStates>
{
  //variabel for Login Screen
  bool isPassword_loginScreen = true;
  TextEditingController textEmailController_loginScreen = TextEditingController();
  TextEditingController textPasswordController_loginScreen = TextEditingController();

  //variabel for Home Screen
  Widget body_homeScreen = ItemModule();
  int currentIndexBottomNavigationBar_homeScreen = 0;
  bool isOpneBottomSheat_homeScreen = false;
  GlobalKey<ScaffoldState> scaffoldKey = GlobalKey();

  //variabel for Item Module
  List<ItemModel> items = [];
  bool isfinish_itemsModule = false;

  //variabel for Category Module
  List<CategoryModel> categories = [];
  bool isfinish_categoryModule = false;

  // variable for cubit
  DioHelper dio = DioHelper();
  UserModel? user;

  //variable for items by catergory id
  int id = 0;
  List<ItemModel> itemsByCategoryId = [];
  bool isComplete = false;

  //variabel for add item Screen
  TextEditingController name_addItemScreen = TextEditingController();
  TextEditingController price_addItemScreen = TextEditingController();
  TextEditingController discription_addItemScreen = TextEditingController();
  TextEditingController expirationDate_AddItemScreen = TextEditingController();
  File? image_addItemScreen;
  CategoryModel? categoryForItem_addItemScreen;
  DateTime? expirationDate_addItemScreen;
  List<Map<String, dynamic>> attributeTypeByCategoryId_addItemScreen = [];
  List<TextEditingController> attributeValuesControllers_addItemScreen = [];

  //variabel for add Category Screen
  File? imageCategory_addCategoryScreen;
  List<String> categoryAttributes_addCategoryScreen = [];

  //variabel for Category Details
  CategoryModel? categorty_categoryDetails;
  List<String> attributeType_categoryDetails = [];

  //variabel for Item Detaile
  ItemModel? item_itemDetails;
  Response? itemAttributesValues_itemsDetailsScreen;

  //variabel for Adddress Screen
  AddressModel? userAddress_addressScreen;
  TextEditingController phoneNumberController_addressScreen = TextEditingController();
  TextEditingController streetAddressController_addressScreen = TextEditingController();
  TextEditingController cityController_addressScreen = TextEditingController();
  TextEditingController recieverNameController_addressScreen = TextEditingController();

  //variabel for Items By User
  List<ItemModel> itemsPostedByUser_itemsByUser = [];

  //variable for search category screen
  List<CategoryModel> category_searchCategoryScreen = [];

  //variabel for search item screen
  List<ItemModel> item_searchItemScreen = [];

  //variabel for your purchases
  ItemsBroughtByMeModel? itemsBroughtByMeModel;

  //variabel for search item category screen
  List<ItemModel> searchItemsCategoryByName_searchItemsByCategoryScreen = [];

  AppCubit() : super(AppInitState());

  static AppCubit get(BuildContext context) => BlocProvider.of(context);

  //Functions for Login and register screen
  void changeSecurity_LoginScreen()
  {
    isPassword_loginScreen = !isPassword_loginScreen;
    emit(AppChangeState());
  }

  void register_registerScreen(BuildContext context, String email, String password, String phone, String profileName)
  {
    emit(AppLoadingState());
    printDebug("Start post new register");
    dio.post_registNewUser(email, password, phone, profileName).then((value)
    {
      printDebug("Success post new register");
      if(value.statusCode == 400)
        {
          Fluttertoast.showToast(
            msg: value.data["errors"][0],
            toastLength: Toast.LENGTH_LONG
          );
        }
      else
        {
          Navigator.pop(context);
        }
      emit(AppChangeState());
    }).catchError((error)
    {
      printDebug("Catch Error");
      printDebug(error.toString());
      emit(AppChangeState());
    });

  }

  void login_loginScreen(String email, String password, BuildContext context)
  {
    emit(AppLoadingState());
    printDebug("Start post Login");
    dio.post_login(email, password).then((value)
    {
      printDebug("Success login");
      Log.v(value.statusCode.toString());
      if(value.statusCode == 400)
        {
          Fluttertoast.showToast(
            msg: value.data["errors"],
            toastLength: Toast.LENGTH_LONG
          );
          Log.w(value.data.toString());
          emit(AppChangeState());
        }
      else if(value.statusCode == 200)
        {
          String token = value.data["token"];
          Map<String, dynamic> jsonToken = JwtDecoder.decode(token);
          user = UserModel.fromJson(value.data, jsonToken);
          CacheHelper.saveToken(user!.token!);
          Log.v(user!.token!);
          CacheHelper.saveRefreshToken(user!.refreshToken!);
          Log.v(user!.refreshToken!);
          Log.v("User is ${AppCubit.get(context).user!.role}");
          Navigator.pushAndRemoveUntil(
            context,
            MaterialPageRoute(builder: (context) => HomeScreen()),
              (route) => false
          );
          getItemsData_itemModule();
          getCategoriesData_categoryModule();
        }
    }).catchError((error)
    {
      printDebug("Catch Error");
      printDebug(error.toString());
      Fluttertoast.showToast(
        msg: "Error",
        toastLength: Toast.LENGTH_LONG
      );
      emit(AppChangeState());
    });
  }



  //function for homeScreen
  void buildItemModule_homeScreen()
  {
    currentIndexBottomNavigationBar_homeScreen = 0;
    body_homeScreen = ItemModule();
    emit(AppChangeState());
  }

  void buildCategoryModule_homeScreen()
  {
    currentIndexBottomNavigationBar_homeScreen = 1;
    body_homeScreen = CategoryModule();
    emit(AppChangeState());
  }

  void buildUserModule_homeScreen()
  {
    body_homeScreen = UserModule();
    currentIndexBottomNavigationBar_homeScreen = 2;
    emit(AppChangeState());
  }


  //function for Item Module
  void getItemsData_itemModule()
  {

    //printDebug("Start get Item data");
    dio.get_item().then((value)
    {
      //printDebug("Success get ItemData");
      //printDebug(value.data.toString());
      if(value.statusCode == 200)
        {
          isfinish_itemsModule = true;
          items.clear();
          for(int i = 0; i < value.data["data"].length; i++)
          {
            items.add(ItemModel.fromJson(value.data["data"][i]));
          }
          //Log.v(items[0].seller!.id);
          //printDebug(items[0].name!);
          //printDebug(items[0].category!.name!);
          //printDebug(items.length.toString());
          emit(AppChangeState());
        }
      else
        {
          Log.w("Request faild ${value.statusCode}");
        }
    }).catchError((e)
    {
      Log.catchE(e);
    });
  }


  //function for category Module
  void getCategoriesData_categoryModule() async
  {
    //printDebug("Start get category");
    dio.get_category().then((value)
    {
      //printDebug("Success get data categoy");
      if(value.statusCode == 200)
        {
          //Log.v("getCategoriesData_categoryModule Status code 200");
          //Log.v(value.data["data"].toString());
          isfinish_categoryModule = true;
          categories.clear();
          for(int i = 0; i < value.data["data"].length; i++)
          {
            categories.add(CategoryModel.fromJson(value.data["data"][i]));
          }
        }
      else
      {
        //Log.w("Error Requested ${value.statusCode}");
      }
      //printDebug("Success parsing");
      //printDebug(categories[0].name!);
      emit(AppChangeState());
    }).catchError((error)
    {
      printDebug("Catch Error");
      printDebug(error.toString());
    });
  }


  //function for add Catrgory Screen
  void postNewCategory_addCategoryScreen(String name, String descreption, String imageUrl, BuildContext context)
  {
    emit(AppLoadingState());
    Log.v("Start Post Category");
    dio.post_category(name, descreption, imageUrl).then((value)
    {
      Log.v("Success Post Category");
      if(value.statusCode == 200)
        {
          Log.v("Success Response");
          //Log.v(value.data.toString());

          Fluttertoast.showToast(msg: "Success Create Category", toastLength: Toast.LENGTH_LONG);
          //post attribute for category
          //Log.v(jsonEncode(categoryAttributes_addCategoryScreen));
          Log.v(value.data["id"].toString());
          Log.v("Start Post Attribute");
          for(int i = 0; i < categoryAttributes_addCategoryScreen.length; i++)
            {
              Dio().post(
                  post_AttributeType,
                  data:
                    [{
                      "categoryId": value.data["id"],
                      "attributeTypeName": categoryAttributes_addCategoryScreen[i].toString()
                    }]
                  ,
                  options: Options(
                      headers: {"Authorization": "bearer ${user!.token}"},
                      contentType: Headers.jsonContentType,
                      responseType: ResponseType.json,
                      validateStatus: (_) => true
                  )
              ).then((value2){
                Log.v("complete post attribute");
                if(value2.statusCode == 200)
                {
                  Log.v(value2.statusCode.toString());
                  Log.v("Success post Attribut");
                }
                else
                {
                  Log.w("Faild post Attribute");
                  Log.w(value2.statusCode.toString());
                  Log.w(value2.data.toString());
                }
              }).catchError((e){
                Log.catchE(e);
              });
            }
          Navigator.pop(context);
          imageCategory_addCategoryScreen = null;
          categoryAttributes_addCategoryScreen.clear();
          getCategoriesData_categoryModule();
        }
      else
        {
          Log.w("Faild Response ${value.statusCode}");
          Log.w(value.data.toString());
          Fluttertoast.showToast(msg: "Error", toastLength: Toast.LENGTH_LONG);
        }
      emit(AppChangeState());
      //Navigator.pop(context);
    }).catchError((e)
    {
      Log.catchE(e);
      emit(AppChangeState());
      Fluttertoast.showToast(msg: "Error", toastLength: Toast.LENGTH_LONG);
    });
  }

  //functions for items by category
  void getItemsByCategoryId_itemsByCategory(int id)
  {
    itemsByCategoryId.clear();

    for(int i = 0; i < items.length; i++)
      {
        if(items[i].category!.id! == id)
          {
            itemsByCategoryId.add(items[i]);
          }
      }
    for(int i = 0; i < itemsByCategoryId.length; i++)
      {
        Log.v("$id ${itemsByCategoryId[i].id} ${itemsByCategoryId[i].name}");
      }
    isComplete = true;
    emit(AppChangeState());
  }

  //function for Add Item Screem
  void postNewItem_addItemScreen(BuildContext context, String name, String description, int price, String imageUrl, int categoryId)
  {
    Log.v(CacheHelper.getToken()!);
    Log.v(imageUrl);
    emit(AppLoadingState());
    Log.v("start Post Item");
    Dio().post(
      post_Item,
      data: {
        "name": name,
        "description": description,
        "price": price,
        "imageUrl": imageUrl,
        "categoryId": categoryId
      },
      options: Options(
        validateStatus: (_)=>true,
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}",
        }
      )
    ).
    then((value)
    {
      Log.v("complete post item");
      {
        if(value.statusCode == 200)
          {
            Log.v("Success Post Item");
            //Log.v(value.data.toString());
            getItemsData_itemModule();
            Fluttertoast.showToast(msg: "Success Create Item", toastLength: Toast.LENGTH_LONG);
            postItemAttribute_addItemScreen(value.data["id"]).then((value)
            {
              Log.v("complete  post Attributessssss");
              Navigator.pop(context);
            }).catchError((e)
            {
              Log.e("Error in post Item");
              Log.catchE(e);
            });
          }
        else
          {
            Log.w("Faild Response ${value.statusCode}");
            Log.w(value.data.toString());
            Log.v(value.statusMessage!);
            Fluttertoast.showToast(msg: "Error", toastLength: Toast.LENGTH_LONG);
          }
        emit(AppChangeState());

      }
    }).catchError((e)
    {
      emit(AppChangeState());
      Log.catchE(e);
      Fluttertoast.showToast(msg: "Error", toastLength: Toast.LENGTH_LONG);
    });
  }
  Future<Response> postImage(File image) async
  {
    FormData formData = FormData.fromMap({
      "file": await MultipartFile.fromFile(image.path,)
    });
    return await Dio().post(
      post_Image,
      data: formData,
      options: Options(
        headers: {
          "Authorization": "bearer ${CacheHelper.getToken()}"
        },
        responseType: ResponseType.plain,
        contentType: Headers.jsonContentType,
        validateStatus: (_) => true
      )
    );
  }
  void getAttributeTypeByCategoryId_addItemScreen(int categoryId)
  {
    attributeTypeByCategoryId_addItemScreen.clear();
    attributeValuesControllers_addItemScreen.clear();
    Log.v("Start get Attribute Type");
    dio.get_attributeType(categoryId).then((value)
    {
      Log.v("complete get attribute");
      if(value.statusCode == 200)
        {
          //Log.v(value.data.toString());
          for(int i = 0; i < value.data.length; i++)
            {
              attributeTypeByCategoryId_addItemScreen.add({
                "id": value.data[i]["id"],
                "name": value.data[i]["name"]
              });
              attributeValuesControllers_addItemScreen.add(TextEditingController());
            }
          Log.v("Lenght of Controller ${attributeValuesControllers_addItemScreen.length}");
          Log.v("Length of Attribute ${attributeTypeByCategoryId_addItemScreen.length}");
          //Log.v(attributeTypeByCategoryId_addItemScreen[0].toString());
          emit(AppChangeState());
        }
      else
        {
          Log.faildResponse(value, "Attribute Type");
        }
    }).catchError((e)
    {
      Log.catchE(e);
    });
  }
  Future postItemAttribute_addItemScreen(int itemId) async
  {
    Log.v("Start post Item Attribute");
    for(int i = 0; i < attributeTypeByCategoryId_addItemScreen.length; i++)
      {
        await Dio().post(
          post_AttributeValue,
          data: [
            {
              "itemId": itemId,
              "attributeTypeId": attributeTypeByCategoryId_addItemScreen[i]["id"],
              "attributeValue": attributeValuesControllers_addItemScreen[i].text.toString()
            }
          ],
          options: Options(
            validateStatus: (_) => true,
            contentType: Headers.jsonContentType,
            responseType: ResponseType.json,
            headers: {
              "Authorization": "bearer ${CacheHelper.getToken()}"
            }
          )
        ).then((value)
        {
          Log.v("Complete Start Attribute");
          if(value.statusCode == 200)
            {
              Log.v("Success post Attribute");
            }
          else
            {
              Log.faildResponse(value, "Post Attribute item");
            }
        }).catchError((e){
          Log.e("Error in post attribute");
          Log.catchE(e);});
      }
  }

  //function for Category Details
  void getAttributeType_categoryDetails()
  {
    attributeType_categoryDetails.clear();
    Dio().get(
      "$get_AttributeType${categorty_categoryDetails!.id}"
    ).then((value)
    {
      if(value.statusCode == 200)
        {
          Log.v("Success get attribute");
          for(int i = 0; i < value.data.length; i++)
            {
              attributeType_categoryDetails.add(value.data[i]["name"]);
            }
          emit(AppChangeState());
        }
      else
        {
          Log.w("Faild get Attribute");
          Log.v(value.statusCode.toString());
          Log.v(value.data.toString());
        }
    }).catchError((e)
    {
      Log.catchE(e);
    });
  }
  void deleteCategory_categoryDetails(BuildContext context, int categoryId)
  {
    Log.v("Start delete Category");
    Log.v("Delete Category Id: $categoryId");
    dio.delete_category(categoryId).then((value)
    {
      Log.v("Complete delete");
      if(value.statusCode == 204)
        {
          Log.v("Success delete category");
          getCategoriesData_categoryModule();
          Navigator.pop(context);
        }
    }).catchError((e)
    {
      Log.e(e);
    });
  }


  //function for Item Details
  void getAttributeValues_itemDetaielsScreen(int itemId)
  {
    dio.getAttributeValues(itemId).then((value)
    {
      if(value.statusCode == 200)
        {
          itemAttributesValues_itemsDetailsScreen = value;
          emit(AppChangeState());
        }
      else
        {
          Log.faildResponse(value, "get Attribute Values");
        }
    }).catchError((e)
    {
      Log.catchE(e);
    });
  }

  void postOrder_itemDetaielsScreen(ItemModel item)
  {
    Log.v("Start post Order");
    
    Dio().post(
      post_CreateAnOrder,
      data: {
        "itemId": item.id,
        "sellerId": item.seller!.id,
        "phoneNumber": userAddress_addressScreen!.phoneNumber,
        "streetAddress": userAddress_addressScreen!.streetAddress,
        "city": userAddress_addressScreen!.city,
        "recieverName": userAddress_addressScreen!.recieverName
      },
      options: Options(
        contentType: Headers.jsonContentType,
        responseType: ResponseType.json,
        validateStatus: (_) => true
      )
    ).then((value)
    {
      if(value.statusCode == 200)
        {}
    }).catchError((e)
    {

    });
  }

  //functions for User Address
  void getUserAddress_addressScreen() async
  {
    Log.v("Start get Address");
    dio.getUserAddress().then((value)
    {
      //Log.v("Complete get Address");
      if(value.statusCode == 200)
        {
          //Log.v("Success get user Address");
          userAddress_addressScreen = AddressModel.fromJson(value.data);
          cityController_addressScreen.text = userAddress_addressScreen!.city;
          phoneNumberController_addressScreen.text = userAddress_addressScreen!.phoneNumber;
          streetAddressController_addressScreen.text = userAddress_addressScreen!.streetAddress;
          recieverNameController_addressScreen.text = userAddress_addressScreen!.recieverName;
        }
      else
        {
          Log.faildResponse(value, "User Address");
        }
    }).catchError((e)
    {
      Log.catchE(e);
    });
  }

  void postUserAddress_addressScreen()
  {
    Log.v("Start Post User Address");
    dio.postUserAddress(phoneNumberController_addressScreen.text.toString(), streetAddressController_addressScreen.text.toString(), cityController_addressScreen.text.toString(), recieverNameController_addressScreen.text.toString()).
    then((value)
    {
      if(value.statusCode == 200)
        {
          Log.v("Success post Address");
        }
      else
        {
          Log.faildResponse(value, "post User Address");
        }
    }).catchError((e){Log.catchE(e);});
  }

  //function for Items by User
  void getItemsPostedByUser()
  {
    emit(AppLoadingState());
    itemsPostedByUser_itemsByUser.clear();
    Log.v("Start get Items Posted By User");
    dio.getItemsPostedByUser().then((value)
    {
      Log.v("complete get Items Posted By User");
      if(value.statusCode == 200)
        {
          Log.v("Success get Items");
          for(int i = 0; i < value.data["data"].length; i++)
            {
              itemsPostedByUser_itemsByUser.add(ItemModel.fromJson(value.data["data"][i]));
            }
          emit(AppChangeState());
        }
      else
        {
          Log.faildResponse(value, "Get Item Posted By User");
          emit(AppChangeState());
        }
    }).catchError((e)
    {
      Log.catchE(e);
      emit(AppChangeState());
    });
  }

  //function for search category screen
  void searchCategory_searchCategoryScreen(String value)
  {
    emit(AppLoadingState());
    category_searchCategoryScreen.clear();
    value = value.toLowerCase();
    for(int i = 0; i < categories.length; i++)
      {
        String name = categories[i].name!;
        name = name.toLowerCase();
        if(name.contains(value))
          {
            category_searchCategoryScreen.add(categories[i]);
          }
      }
    emit(AppChangeState());
  }

  //function for search Item Screeb
  void searchItem_searchItemScreen(String value)
  {
    emit(AppLoadingState());
    item_searchItemScreen.clear();
    value = value.toLowerCase();
    for(int i = 0; i < items.length; i++)
    {
      String name = items[i].name!;
      name = name.toLowerCase();
      if(name.contains(value))
      {
        item_searchItemScreen.add(items[i]);
      }
    }
    emit(AppChangeState());
  }

  //function for order Items
  void postOrdee_order(BuildContext context, ItemModel item)
  {
    dio.getUserAddress().then((value)
    {
      //Log.v("Complete get Address");
      if(value.statusCode == 200)
      {
        //Log.v("Success get user Address");
        userAddress_addressScreen = AddressModel.fromJson(value.data);
        Log.v("Start post order");
        dio.postOrder(item.id!,
            item.seller!.id,
            userAddress_addressScreen!.phoneNumber,
            userAddress_addressScreen!.streetAddress,
            userAddress_addressScreen!.city,
            userAddress_addressScreen!.recieverName).
        then((value2)
        {
          if(value2.statusCode == 200)
          {
            Log.v("Sucess post order");
            getItemsData_itemModule();
            Navigator.pop(context);
          }
          else if(value2.statusCode == 400)
          {
            Log.faildResponse(value, "post order");
            Fluttertoast.showToast(msg: value.data.toString(), toastLength: Toast.LENGTH_LONG);
          }
          else
          {
            Log.faildResponse(value2, "post order");
          }
        }).catchError((e)
        {
          Log.catchE(e);
        });
      }
      else
      {
        Log.faildResponse(value, "User Address");
      }
    }).catchError((e)
    {
      Log.catchE(e);
    });
  }

  //function for your purchases Screen
  void getItemsBroughtByUser_yourPurchasesScreen()
  {
    emit(AppLoadingState());
    Log.v("start get Items bought by me");
    dio.getBoughtOrders().then((value)
    {
      Log.v("Complete et Items bought by me");
      if(value.statusCode == 200)
        {
          Log.v("Sucess et Items bought by me");
          itemsBroughtByMeModel = ItemsBroughtByMeModel.fromJson(value);
        }
      else
        {
          Log.faildResponse(value, "items bought by me");
        }
      emit(AppChangeState());
    }).catchError((e)
    {
      Log.catchE(e);
      emit(AppChangeState());
    });
  }

  //function for search item By Category screen
  void searchItemCategoryByName_searchItemCategoryScreen(String name)
  {
    emit(AppLoadingState());
    searchItemsCategoryByName_searchItemsByCategoryScreen.clear();
    name = name.toLowerCase();
    for(int i = 0; i < itemsByCategoryId.length; i++)
      {
        String value = itemsByCategoryId[i].name!;
        value = value.toLowerCase();
        if(value.contains(name))
          {
            searchItemsCategoryByName_searchItemsByCategoryScreen.add(itemsByCategoryId[i]);
          }
      }
    //Log.v(searchItemsCategoryByName_searchItemsByCategoryScreen.toString());
    emit(AppChangeState());
  }
}

//ImCG4IInMiIAZM1tIW1KHn4e1JM=