Chroma Key Demo
===============

Chroma Key Demo is a Nokia Developer example demonstrating the Nokia Imaging SDK chroma key filter.

Tap on the upper original viewfinder to select a chroma key color (for example a green backdrop canvas)
and use the color distance slider to adjust the threshold of the color range to be made transparent.
Tap on the lower modified viewfinder to toggle between a video background and a "transparent" background.

This example application is hosted in GitHub:
https://github.com/nokia-developer/chroma-key-demo/

Developed with Microsoft Visual Studio Express for Windows Phone 2012.

Compatible with:

 * Windows Phone 8

Tested to work on:

 * Nokia Lumia 520
 * Nokia Lumia 1020
 * Nokia Lumia 1520


Instructions
------------

Make sure you have the following installed:

* Windows 8
* Windows Phone SDK 8.0
* Nuget 2.7+

To build and run the sample:

1. Open the SLN file:
   File > Open Project, select the solution (.sln postfix) file
2. Select the target 'Device' and platform 'ARM'.
3. Press F5 to build the project and run it on device.

If the project does not compile on the first attempt it's possible that you
did not have the required packages yet. With Nuget 2.7 or later the missing
packages are fetched automatically when build process is invoked, so try
building again. If some packages cannot be found there should be an
error stating this in the Output panel in Visual Studio Express.

For more information on deploying and testing applications see:
http://msdn.microsoft.com/library/windowsphone/develop/ff402565(v=vs.105).aspx


About the implementation
------------------------

Important folders:

| Folder | Description |
| ------ | ----------- |
| / | Contains the project file, the license information and this file (README.md) |
| ChromaKeyDemo | Root folder for the implementation files.  |
| ChromaKeyDemo/Assets | Graphic assets like icons and tiles. |
| ChromaKeyDemo/Properties | Application property files. |
| ChromaKeyDemo/Resources | Application resources. |

Important files:

| File | Description |
| ---- | ----------- |
| Mainpage.cs | Renders camera live feed to an Image removing selected color from the image. |

Important classes:

| Class | Description |
| ----- | ----------- |
| MainPage | Renders camera live feed to an Image removing selected color from the image. |


Known issues
------------

None.


License
-------

    Copyright © 2013 Nokia Corporation. All rights reserved.
    
    Nokia, Nokia Developer, and HERE are trademarks and/or registered trademarks of
    Nokia Corporation. Other product and company names mentioned herein may be
    trademarks or trade names of their respective owners.
    
    License
    Subject to the conditions below, you may use, copy, modify and/or merge copies
    of this software and associated content and documentation files (the “Software”)
    to test, develop, publish, distribute, sub-license and/or sell new software
    derived from or incorporating the Software, solely in connection with Nokia
    devices. Some of the documentation, content and/or software maybe licensed under
    open source software or other licenses. To the extent such documentation,
    content and/or software are included, licenses and/or other terms and conditions
    shall apply in addition and/or instead of this notice. The exact terms of the
    licenses, disclaimers, acknowledgements and notices are reproduced in the
    materials provided, or in other obvious locations. No other license to any other
    intellectual property rights is granted herein.
    
    This file, unmodified, shall be included with all copies or substantial portions
    of the Software that are distributed in source code form.
    
    The Software cannot constitute the primary value of any new software derived
    from or incorporating the Software.
    
    Any person dealing with the Software shall not misrepresent the source of the
    Software.
    
    Disclaimer
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
    FOR A PARTICULAR PURPOSE, QUALITY AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES (INCLUDING,
    WITHOUT LIMITATION, DIRECT, SPECIAL, INDIRECT, PUNITIVE, CONSEQUENTIAL,
    EXEMPLARY AND/ OR INCIDENTAL DAMAGES) OR OTHER LIABILITY, WHETHER IN AN ACTION
    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
    SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    
    Nokia Corporation retains the right to make changes to this document at any
    time, without notice.


Background video attribution
----------------------------

The video oceantrip-small.mp4 used in this application is a short clip from
http://www.youtube.com/watch?v=nzgfOBTNye0. The original video is copyright of the
"zero-project" (http://www.zero-project.gr/) and licensed under the Creative Commons
Attribution 3.0 Unported (CC BY 3.0) license (http://creativecommons.org/licenses/by/3.0/).


Version history
---------------

* 1.0: First public release of Chroma Key Demo
