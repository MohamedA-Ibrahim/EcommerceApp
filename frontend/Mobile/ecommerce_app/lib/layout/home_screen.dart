import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ecommerce_app/layout/add_item_screen.dart';
import 'package:ecommerce_app/layout/login_screen.dart';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:ecommerce_app/share/cash_helper.dart';
import 'package:ecommerce_app/testing.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

import 'add_category_screen.dart';

class HomeScreen extends StatelessWidget
{
  @override
  Widget build(BuildContext context)
  {

    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state)
      {
        AppCubit cubit = AppCubit.get(context);
        return Scaffold(
          key: cubit.scaffoldKey,
          backgroundColor: Colors.white,
          appBar: AppBar(
            title: Text(
              "E-commerce",
            ),
            actions: [
              IconButton(
                onPressed: ()
                {
                  CacheHelper.removeIsAdmin();
                  CacheHelper.removeRefreshToken();
                  CacheHelper.removeToken();
                  Navigator.pushAndRemoveUntil(context, MaterialPageRoute(builder: (context) => LoginScreen()), (route) => false);
                },
                icon: Icon(Icons.logout),
              ),
            ],
          ),
          body: cubit.body_homeScreen,
          bottomNavigationBar: BottomNavigationBar(
            onTap: (index)
            {
              if(index == 0)
                {
                  cubit.buildItemModule_homeScreen();
                }
              else if(index == 1)
                {
                  cubit.buildCategoryModule_homeScreen();
                }
            },
            currentIndex: cubit.currentIndexBottomNavigationBar_homeScreen,
            showSelectedLabels: true,
            showUnselectedLabels: true,
            selectedItemColor: Colors.blue,
            unselectedItemColor: Colors.black,
            items: [
              BottomNavigationBarItem(
                label: "Items",
                icon: Icon(Icons.menu_sharp)
              ),
              BottomNavigationBarItem(
                label: "Category",
                icon: Icon(Icons.category_rounded)
              ),
            ],
          ),
          floatingActionButton: FloatingActionButton(
            onPressed: ()
            {

              if(cubit.user!.role! == "Admin")
                {
                  if(cubit.isOpneBottomSheat_homeScreen)
                    {
                      Navigator.pop(context);
                      cubit.isOpneBottomSheat_homeScreen = false;
                    }
                  else
                    {
                      cubit.isOpneBottomSheat_homeScreen = true;
                      cubit.scaffoldKey.currentState!.showBottomSheet((context)
                      {
                        return SafeArea(
                          child: Wrap(
                            children: [
                              ListTile(
                                leading: Icon(Icons.menu_sharp),
                                title: Text("Item"),
                                onTap: ()
                                {
                                  Navigator.push(context, MaterialPageRoute(builder: (context)=> AddItemScreen())).then((value) => Navigator.pop(context));
                                },
                              ),
                              ListTile(
                                leading: Icon(Icons.category_rounded),
                                title: Text("Category"),
                                onTap: ()
                                {
                                  Navigator.push(context, MaterialPageRoute(builder: (context)=> AddCategoryScreen())).then((value) => Navigator.pop(context));
                                },
                              )
                            ],
                          ),
                        );
                      }).closed.then((value)
                      {
                        cubit.isOpneBottomSheat_homeScreen = false;
                      });
                    }
                }
              else
                {
                  Navigator.push(context, MaterialPageRoute(builder: (context) => AddItemScreen()));
                }
            },
            child: Icon(
              Icons.add
            ),
          ),
        );
      },
    );
  }
}
