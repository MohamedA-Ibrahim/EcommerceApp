import 'package:ecommerce_app/layout/home_screen.dart';
import 'package:ecommerce_app/layout/on_bording_screen.dart';
import 'package:ecommerce_app/layout/register_screen.dart';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:ecommerce_app/share/cash_helper.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'model/user_model.dart';
void main()async
{
  WidgetsFlutterBinding.ensureInitialized();
  await CacheHelper.init();
  runApp(MyApp());
}
void printDebug(String msg)
{
  print("//////////////////////////////////|||-->> $msg");
}
class MyApp extends StatelessWidget
{
  @override
  Widget build(BuildContext context)
  {

    return BlocProvider(
      create: (context) => AppCubit()..getItemsData_itemModule()..getCategoriesData_categoryModule(),
      child: BlocConsumer<AppCubit, AppStates>(
        listener: (context, state){},
        builder: (context, state)
        {
          Widget home = OnBordingScreen();
          String? token = CacheHelper.getToken();
          if(token != null)
          {
            printDebug("Token is not Null");
            home = HomeScreen();
            AppCubit.get(context).user = UserModel(CacheHelper.getToken(), CacheHelper.getRefreshToken());
          }
          return MaterialApp(
            debugShowCheckedModeBanner: false,
            home: home,
          );
        },
      ),
    );
  }


}