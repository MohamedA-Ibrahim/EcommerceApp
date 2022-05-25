import 'package:flutter/material.dart';

class UserModule extends StatefulWidget {
  const UserModule({Key? key}) : super(key: key);

  @override
  State<UserModule> createState() => _UserModuleState();
}

class _UserModuleState extends State<UserModule> {
  @override
  Widget build(BuildContext context) {
    return Center(
      child: Text("Hello"),
    );
  }
}
