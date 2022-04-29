import 'package:ecommerce_app/layout/login_screen.dart';
import 'package:flutter/material.dart';
import 'package:smooth_page_indicator/smooth_page_indicator.dart';
class OnBordingScreen extends StatefulWidget
{
  @override
  State<OnBordingScreen> createState() => _OnBordingScreen();
}

class _OnBordingScreen extends State<OnBordingScreen>
{
  int currentPageIndex = 0;
  bool isLast = false;
  PageController controller = PageController();
  @override
  Widget build(BuildContext context)
  {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.white,
        elevation: 0.0,
        actions: [
          TextButton(
            onPressed: ()
            {
              Navigator.pushAndRemoveUntil(
                  context,
                  MaterialPageRoute(builder: (context) => LoginScreen()),
                      (route) => false
              );
            },
            child: Text(
              "SKIP",
              style: TextStyle(
                color: Colors.blue
              ),
            ),
          )
        ],
      ),
      body: Container(
        padding: EdgeInsets.all(8),
        color: Colors.white,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Text(
              "Welcome to E-Commerce",
              style: TextStyle(
                color: Colors.black,
                fontWeight: FontWeight.bold,
                fontSize: 15
              ),
            ),
            SizedBox(height: 8,),
            Expanded(
                child: PageView.builder(
                  onPageChanged: (index)
                  {
                    if(index == 3)
                      {
                        setState(() {
                          isLast = true;
                        });
                      }
                    else
                      {
                        setState(() {
                          isLast = false;
                        });
                      }
                  },
                  physics: BouncingScrollPhysics(),
                  controller: controller,
                  itemBuilder: (context, index) => buildPageViewItem(index+1),
                  itemCount: 4,
                )
            ),
            SizedBox(height: 8,),
            Row(
              children: [
                SmoothPageIndicator(
                  controller: controller,
                  count: 4,
                  effect: JumpingDotEffect(
                    dotWidth: 15,
                    dotHeight: 15,
                    activeDotColor: Colors.blue,
                    spacing: 5
                  ),
                  onDotClicked: (index)
                  {
                    controller.animateToPage(index, duration: Duration(milliseconds: 500), curve: Curves.fastLinearToSlowEaseIn);
                  },
                ),
                Spacer(),
                FloatingActionButton(
                  onPressed: ()
                  {
                    if(isLast)
                      {
                        Navigator.pushAndRemoveUntil(
                            context,
                            MaterialPageRoute(builder: (context) => LoginScreen()),
                            (route) => false
                        );
                      }
                    else
                      {
                        controller.nextPage(duration: Duration(milliseconds: 500), curve: Curves.fastLinearToSlowEaseIn);
                      }
                  },
                  child: Icon(
                    Icons.arrow_forward_ios
                  ),
                )
              ],
            )
          ],
        ),
      ),
    );
  }

  Widget buildPageViewItem(int index)
  {
    currentPageIndex = index;
    return Image(
      image: AssetImage("assets/images/${index.toString()}.png"),
      fit: BoxFit.cover,
    );
  }
}