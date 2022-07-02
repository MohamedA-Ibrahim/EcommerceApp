import 'package:ecommerce_app/layout/create_order_screen.dart';
import 'package:ecommerce_app/layout/items_by_user.dart';
import 'package:ecommerce_app/layout/order_created_by_loged_user_screen.dart';
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
                    cubit.getItemsPostedByUser();
                    Navigator.push(context, MaterialPageRoute(builder: (context) => ItemsByUserScreen()));
                  },
                  child: Container(
                    width: double.infinity,
                    height: 40,
                    color: Colors.blue,
                    child: Center(
                      child: Text(
                        "Your Items",
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
                    cubit.getMyOrders_orderCreatedByLogedScreen();
                    Navigator.push(context, MaterialPageRoute(builder: (context) => OrderCreatedByLogedUserScreen()));
                  },
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
