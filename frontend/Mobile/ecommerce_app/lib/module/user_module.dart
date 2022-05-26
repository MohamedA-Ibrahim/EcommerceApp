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
        return Container(
          height: double.infinity,
          padding: EdgeInsets.all(16),
          child: Center(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                TextButton(
                  onPressed: (){},
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
                  onPressed: (){},
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
                  onPressed: (){},
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
                )
              ],
            ),
          ),
        );
      },
    );
  }
}
