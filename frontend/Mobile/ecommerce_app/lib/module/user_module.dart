import 'package:ecommerce_app/layout/address_screen.dart';
import 'package:ecommerce_app/layout/items_by_user.dart';
import 'package:ecommerce_app/layout/your_purchases_screen.dart';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class UserModule extends StatefulWidget {
  const UserModule({Key? key}) : super(key: key);

  @override
  State<UserModule> createState() => _UserModuleState();
}

class _UserModuleState extends State<UserModule> {
  @override
  Widget build(BuildContext context) {
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state)
      {
        AppCubit cubit = AppCubit.get(context);
        return Container(
          height: double.infinity,
          padding: EdgeInsets.all(16),
          child: Center(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                TextButton(
                  onPressed: ()
                  {
                    cubit.getUserAddress_addressScreen();
                    Navigator.push(context, MaterialPageRoute(builder: (context) => AddressScreen()));
                  },
                  child: Container(
                    width: double.infinity,
                    height: 40,
                    color: Colors.blue,
                    child: Center(
                      child: Text(
                        "Update Address",
                        style: TextStyle(
                            color: Colors.white
                        ),
                      ),
                    ),
                  ),
                ),
                TextButton(
                  onPressed: ()
                  {
                    cubit.getItemsPostedByUser();
                    Navigator.push(context, MaterialPageRoute(builder: (context) => ItemsByUserScreen()));
                  },
                  child: Container(
                    width: double.infinity,
                    height: 40,
                    color: Colors.blue,
                    child: Center(
                      child: Text(
                        "Your advertisement",
                        style: TextStyle(
                            color: Colors.white
                        ),
                      ),
                    ),
                  ),
                ),
                TextButton(
                  onPressed: ()
                  {
                    cubit.getItemsBroughtByUser_yourPurchasesScreen();
                    Navigator.push(context, MaterialPageRoute(builder: (context) => YourPurchasesScreen()));
                  },
                  child: Container(
                    width: double.infinity,
                    height: 40,
                    color: Colors.blue,
                    child: Center(
                      child: Text(
                        "your purchases",
                        style: TextStyle(
                            color: Colors.white
                        ),
                      ),
                    ),
                  ),
                ),
                TextButton(
                  onPressed: (){},
                  child: Container(
                    width: double.infinity,
                    height: 40,
                    color: Colors.blue,
                    child: Center(
                      child: Text(
                        "Orders",
                        style: TextStyle(
                            color: Colors.white
                        ),
                      ),
                    ),
                  ),
                )
              ],
            ),
          ),
        );
      },
    );
  }
}
