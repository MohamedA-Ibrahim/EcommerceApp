class UserModel
{
  String? token;
  String? refreshToken;
  String? email;
  String? phone;
  String? profileName;
  String? role;

  UserModel(this.token, this.refreshToken, Map<String, dynamic> jwt)
  {
    email = jwt["email"];
    phone = jwt["phone"];
    profileName = jwt["profileName"];
    role = jwt["role"];
  }
  UserModel.fromJson(Map<String, dynamic> json, Map<String, dynamic> jwt)
  {
    token = json["token"];
    refreshToken = json["refreshToken"];
    email = jwt["email"];
    phone = jwt["phone"];
    profileName = jwt["profileName"];
    role = jwt["role"];
  }
}