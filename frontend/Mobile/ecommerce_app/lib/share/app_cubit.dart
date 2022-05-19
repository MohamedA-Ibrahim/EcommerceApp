

import 'dart:io';

import 'package:dio/dio.dart';
import 'package:ecommerce_app/Network/dio_helper.dart';
import 'package:ecommerce_app/layout/home_screen.dart';
import 'package:ecommerce_app/main.dart';
import 'package:ecommerce_app/model/category_model.dart';
import 'package:ecommerce_app/model/item_model.dart';
import 'package:ecommerce_app/model/user_model.dart';
import 'package:ecommerce_app/module/items_module.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:ecommerce_app/share/cash_helper.dart';
import 'package:ecommerce_app/testing.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_facebook_auth/flutter_facebook_auth.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';
import 'package:permission_handler/permission_handler.dart';

import '../module/categoru_module.dart';

class AppCubit extends Cubit<AppStates>
{
  //for Login Screen
  bool isPassword_loginScreen = true;
  TextEditingController textEmailController_loginScreen = TextEditingController();
  TextEditingController textPasswordController_loginScreen = TextEditingController();

  // for Home Screen
  Widget body_homeScreen = ItemModule();
  int currentIndexBottomNavigationBar_homeScreen = 0;
  bool isOpneBottomSheat_homeScreen = false;
  GlobalKey<ScaffoldState> scaffoldKey = GlobalKey();

  //for Item Module
  List<ItemModel> items = [];
  bool isfinish_itemsModule = false;

  //for Category Module
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

  AppCubit() : super(AppInitState());

  static AppCubit get(BuildContext context) => BlocProvider.of(context);

  //Functions for Login and register screen
  void changeSecurity_LoginScreen()
  {
    isPassword_loginScreen = !isPassword_loginScreen;
    emit(AppChangeState());
  }

  void register_registerScreen(BuildContext context, String email, String password)
  {
    //   "email": "ffba@example.com",
    // "password": "Fafa_123456789"
    // emit(AppLoadingState());
    // printDebug("Start POST");
    // Dio dio = Dio(BaseOptions(
    //   contentType: Headers.jsonContentType,
    //   responseType: ResponseType.json,
    //   validateStatus: (_)=>true,));
    // dio.post(
    //   "https://ecommerceapiservice.azurewebsites.net/api/v1/identity/register",
    //   data: {
    //     "email": email,
    //     "password": password
    //   }
    // ).then((value)
    // {
    //   printDebug("success Post");
    //   printDebug(value.data.toString());
    //   emit(AppChangeState());
    // }).catchError((error)
    // {
    //   printDebug("Catch Error");
    //   printDebug(error.toString());
    //   emit(AppChangeState());
    // });
    emit(AppLoadingState());
    printDebug("Start post new register");
    dio.post_registNewUser(email, password).then((value)
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
      if(value.statusCode == 400)
        {
          Fluttertoast.showToast(
            msg: value.data["errors"],
            toastLength: Toast.LENGTH_LONG
          );
          emit(AppChangeState());
        }
      else
        {
          if(email == "admin@gmail.com")
            {
              CacheHelper.setAdmin(true);
              Log.v("admin is true");
            }
          else
            {
              CacheHelper.setAdmin(false);
              Log.v("admin is false");
            }
          user = UserModel.fromJson(value.data);
          CacheHelper.saveToken(user!.token!);
          Log.v(user!.token!);
          CacheHelper.saveRefreshToken(user!.refreshToken!);
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

  void facebookLogin_loginScreen()
  {
    printDebug("Strart Login with facebook");

    FacebookAuth.i.login().then((value)
    {
      printDebug("Success Login");
      if(value.status == LoginStatus.success)
        {
          printDebug("Seccess LoginStatus.success");
        }
      else if(value.status == LoginStatus.cancelled)
        {
          printDebug("Seccess LoginStatus.cancelled");
        }
      else if(value.status == LoginStatus.failed)
        {
          printDebug("Seccess LoginStatus.failed");
        }
    }).catchError((error){
      printDebug("CatchError loginFacebook");
      printDebug(error.toString());
    });
    // FacebookLogin().logIn(["email"]).then((value)
    // {
    //   printDebug("Sucess login");
    //   if(value.status == FacebookLoginStatus.loggedIn)
    //     {
    //       printDebug("msg FacebookLoginStatus.loggedIn");
    //       printDebug(value.accessToken.toString());
    //     }
    //   else if(value.status == FacebookLoginStatus.error)
    //     {
    //       printDebug("msg FacebookLoginStatus.error");
    //     }
    // }).catchError((error)
    // {
    //   printDebug("Catch Error");
    //   printDebug(error.toString());
    // });
  }
///////////////////////////////////////////////////////////////////////////////////////////

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
  ////////////////////////////////////////////////////////////////////////////////////////

  //function for Item Module
  void getItemsData_itemModule()
  {

    printDebug("Start get Item data");
    dio.get_item().then((value)
    {
      printDebug("Success get ItemData");
      //printDebug(value.data.toString());
      if(value.statusCode == 200)
        {
          isfinish_itemsModule = true;
          for(int i = 0; i < value.data.length; i++)
          {
            items.add(ItemModel.fromJson(value.data[i]));
          }
          printDebug("Success parsing");
          //printDebug(items[0].name!);
          //printDebug(items[0].category!.name!);
          printDebug(items.length.toString());
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
////////////////////////////////////////////////////////////////////////////////////////

  //function for category Module
  void getCategoriesData_categoryModule() async
  {
    printDebug("Start get category");
    dio.get_category().then((value)
    {
      printDebug("Success get data categoy");
      if(value.statusCode == 200)
        {
          isfinish_categoryModule = true;
          for(int i = 0; i < value.data.length; i++)
          {
            categories.add(CategoryModel.fromJson(value.data[i]));
          }
        }
      else
      {
        Log.w("Error Requested ${value.statusCode}");
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

////////////////////////////////////////////////////////////////////////////////////////
  //function for add Catrgory Screen
  void postNewCategory_addCategoryScreen(String name, BuildContext context)
  {
    emit(AppLoadingState());
    Log.v("Start Post Category");
    dio.post_category(name).then((value)
    {
      Log.v("Success Post Category");
      if(value.statusCode == 200)
        {
          Log.v("Success Response");
          Log.v(value.data.toString());
          getCategoriesData_categoryModule();
          Fluttertoast.showToast(msg: "Success Create Category", toastLength: Toast.LENGTH_LONG);
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
        if(items[i].categoryId! == id)
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
  void postNewItem_addItemScreen(String name, String description, int price, String imageUrl, int categoryId, DateTime expirationDate)
  {
    Log.v(CacheHelper.getToken()!);
    Log.v(imageUrl);
    emit(AppLoadingState());
    Log.v("start Post Item");
    Dio().post(
      "https://ecommerceapiservice.azurewebsites.net/api/v1/items",
      data: {
        "name": name,
        "description": description,
        "price": price,
        "imageUrl": imageUrl,
        "categoryId": categoryId,
        "expirationDate": "2022-04-29T02:12:46.483Z"
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
      Log.v("Success post item");
      {
        if(value.statusCode == 200)
          {
            Log.v("Success Response");
            Log.v(value.data.toString());
            getItemsData_itemModule();
            Fluttertoast.showToast(msg: "Success Create Item", toastLength: Toast.LENGTH_LONG);
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
      "https://ecommerceapiservice.azurewebsites.net/api/v1/images",
      data: formData
    );
  }

}

//ImCG4IInMiIAZM1tIW1KHn4e1JM=