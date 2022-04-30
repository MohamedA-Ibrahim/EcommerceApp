import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:ecommerce_app/share/share_componant.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:fluttertoast/fluttertoast.dart';

import '../main.dart';

class RegisterScreen extends StatelessWidget
{
  TextEditingController textEmailController = TextEditingController();
  TextEditingController textPasswordController = TextEditingController();
  GlobalKey<FormState> formKey = GlobalKey();
  @override
  Widget build(BuildContext context)
  {
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state){
        return Scaffold(
          appBar: AppBar(
            title: Text(
              "Register"
            ),
          ),
          body: Padding(
            padding: const EdgeInsets.all(16.0),
            child: Form(
              key: formKey,
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  Text(
                    "Create New User",
                    style: Theme.of(context).textTheme.headline5,
                  ),
                  SizedBox(height: 15,),
                  defaultFormField(
                      controller: textEmailController,
                      type: TextInputType.emailAddress,
                      validate: (value){
                        if(value == null || value.isEmpty)
                          {
                            return "Enter your email";
                          }
                      },
                      label: "Email",
                      prefix: Icons.email_outlined
                  ),
                  SizedBox(height: 15,),
                  defaultFormField(
                      controller: textPasswordController,
                      type: TextInputType.text,
                      validate: (value){
                        if(value == null || value.isEmpty)
                          {
                            return "Enter your password";
                          }
                      },
                      label: "Password",
                      prefix: Icons.lock_clock_outlined
                  ),
                  SizedBox(height: 15,),
                  conditionBuilder(context, (state is AppLoadingState))
                ],
              ),
            ),
          )
        );
      },
    );
  }

  Widget conditionBuilder(BuildContext context, bool flag)
  {
    AppCubit cubit = AppCubit.get(context);
    if(flag)
    {
      return Center(child: CircularProgressIndicator(),);
    }
    else
    {
      return TextButton(
        onPressed: ()
        {
          if(formKey.currentState!.validate())
            {
              String email = textEmailController.text.toString();
              String password = textPasswordController.text.toString();
              cubit.register_registerScreen(context, email, password);
            }
        },
        child: Container(
          width: double.infinity,
          height: 40,
          color: Colors.blue,
          child: Center(
            child: Text(
              "REGIST NOW",
              style: TextStyle(
                  color: Colors.white
              ),
            ),
          ),
        ),
      );
    }
  }
}