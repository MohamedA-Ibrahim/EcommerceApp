import 'package:ecommerce_app/layout/register_screen.dart';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../main.dart';
import '../share/share_componant.dart';

class LoginScreen extends StatelessWidget
{
  GlobalKey<FormState> formKey = GlobalKey();
  @override
  Widget build(BuildContext context)
  {
    AppCubit cubit = AppCubit.get(context);
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state)
      {
        AppCubit cubit = AppCubit.get(context);
        return Scaffold(
          appBar: AppBar(
            title: Text(
                "E-Commerce"
            ),
          ),
          body: Center(
            child: SingleChildScrollView(
              child: Padding(
                padding: const EdgeInsets.all(20.0),
                child: Form(
                  key: formKey,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text(
                        "LOGIN",
                        style: Theme.of(context).textTheme.headline5,
                      ),
                      SizedBox(height: 15,),
                      defaultFormField(
                          controller: cubit.textEmailController_loginScreen,
                          type: TextInputType.emailAddress,
                          label: "Email",
                          validate: (value)
                          {
                            if(value != null && value.isEmpty)
                            {
                              return "Enter you Email";
                            }
                          },
                          prefix: Icons.email_outlined
                      ),
                      SizedBox(height: 15,),
                      defaultFormField(
                        label: "Password",
                        controller: cubit.textPasswordController_loginScreen,
                        prefix: Icons.lock_clock_outlined,
                        isPassword: cubit.isPassword_loginScreen,
                        type: TextInputType.visiblePassword,
                        validate: (value)
                        {
                          if(value != null &&value.isEmpty)
                          {
                            return "Enter your Password";
                          }
                        },
                        suffixPressed: ()
                        {
                          AppCubit.get(context).changeSecurity_LoginScreen();
                        },
                        suffix: Icons.visibility_off_outlined,
                      ),
                      SizedBox(height: 15,),
                      conditionBuilder(context, (state is AppLoadingState)),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Text(
                              "Don't have an account?"
                          ),
                          TextButton(
                            onPressed: ()
                            {
                              Navigator.push(context, MaterialPageRoute(builder: (context) => RegisterScreen()));
                            },
                            child: Text(
                                "Register now"
                            ),
                          )
                        ],
                      )
                    ],
                  ),
                ),
              ),
            ),
          ),
        );
      },
    );
  }

  Widget conditionBuilder(BuildContext context, bool flag)
  {
    if(flag)
    {
      return Center(child: CircularProgressIndicator(),);
    }
    else
    {
      return TextButton(
        onPressed: ()
        {
          AppCubit cubit = AppCubit.get(context);
          printDebug("on pressed Login");
          if(formKey.currentState!.validate())
          {
            String email = cubit.textEmailController_loginScreen.text.toString();
            String password = cubit.textPasswordController_loginScreen.text.toString();
            cubit.login_loginScreen(email, password, context);
          }
        },
        child: Container(
          width: double.infinity,
          height: 40,
          color: Colors.blue,
          child: Center(
            child: Text(
              "LOGIN",
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