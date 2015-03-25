# Instant-Tracking-Sample
<div dir="ltr" style="text-align: left;" trbidi="on">
<div dir="ltr" style="text-align: left;" trbidi="on">
<h2 style="text-align: left;">
<span style="color: #073763;"><span style="font-family: Comic Sans MS;"><span style="font-size: large;"><u><b>Instant Tracking</b></u></span></span></span></h2>
<div class="separator" style="clear: both; text-align: center;">
</div>
<div style="text-align: left;">
<br /></div>
<div style="text-align: left;">
<span style="color: #073763;"><span style="font-family: Comic Sans MS;"><span style="font-size: large;"><span style="font-size: small;">&nbsp;This is a sample project for using <u>Marker based Tracking</u>.</span></span></span></span></div>
<div style="text-align: left;">
<br /></div>
<div style="text-align: left;">
<span style="color: #073763;"><span style="font-family: Comic Sans MS;"><span style="font-size: large;"><span style="font-size: small;"><u>SDK:</u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Metaio</b></span></span></span></span></div>
<div style="text-align: left;">
<span style="color: #073763;"><span style="font-family: Comic Sans MS;"><span style="font-size: large;"><span style="font-size: small;"><u>Render Software:</u>&nbsp; <b>Unity3D</b></span></span></span></span></div>
<div style="text-align: left;">
<span style="color: #073763;"><span style="font-family: Comic Sans MS;"><span style="font-size: large;"><span style="font-size: small;"><u>AI (Unity Plugin):</u>&nbsp;&nbsp; <b>R.A.I.N</b> AI Engine</span></span></span></span></div>
<div style="text-align: left;">
<span style="color: #073763;"><span style="font-family: Comic Sans MS;"><span style="font-size: large;"><span style="font-size: small;"><u>Script Language:</u>&nbsp;&nbsp;&nbsp; <b>C#</b></span></span></span></span></div>
<div style="text-align: left;">
<a href="https://github.com/mamdouht/Instant-Tracking-Sample/" style="font-family: 'Comic Sans MS';">Project Files</a><br />
<span style="font-size: large;"><span style="font-size: small;"><br /></span></span>
<span style="color: #073763;"><span style="font-family: Comic Sans MS;"><span style="font-size: large;"><span style="font-size: small;">If you are unfamiliar with Augmented Reality, <a href="http://blog.tarabishi.me/2015/03/microsoft-developers-uae-meetup-6-Augmented-Reality.html">this article </a>could introduce AR to you</span></span></span></span></div>
<div style="text-align: left;">
<span style="color: #073763; font-family: 'Comic Sans MS';"><br /></span>
<span style="color: #073763; font-family: 'Comic Sans MS';">To Test the sample:</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">-Make sure you have a camera connected to your system, it is preferred if it is not fixed easier to control</span><br />
<span style="color: #073763; font-family: Comic Sans MS;"><br /></span>
<span style="color: #073763; font-family: Comic Sans MS;">-Open Unity3D with OpenGL Render mode, use the command "Unity.exe -force-opengl"</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">-Scene is in "Assets\Scenes" Folder Double click on "Main" to open the project</span><br />
<span style="color: #073763; font-family: Comic Sans MS;"><br /></span>
<span style="color: #073763; font-family: Comic Sans MS;">-After Opening the Scene:</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">expand MetaioSDK object:</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">you will find two objects with the name "MetaioTracker"</span><br />
<span style="color: #073763; font-family: Comic Sans MS;"><br /></span>
<span style="color: #073763; font-family: Comic Sans MS;">if you click on any of them</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">we will see in the inspector</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">"Coordinate System ID"</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">This is the Id number which specify which Marker image is associated with this marker.</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">This information is predefined in the XML File "TrackingData_MarkerlessFast"</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">Located at the same folder as the markers "Asset\StreamingAssets\TrackingSamples"&nbsp;</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">You may notice also the T1 &amp; T2 Images in that folder.&nbsp;<b><u>The markers &amp; the images there are no need for them but they exist because I copied the XML file from the Previous sample "Marker Based Tracking Sample"</u></b></span><br />
<br />
<span style="color: #073763; font-family: Comic Sans MS;">if you expand the "MetaioTracker" objects you will find:</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">in one of them&nbsp;</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">an object called "Max"</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">and in the other one&nbsp;</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">an object called "Tyrant Zombie"</span><br />
<span style="color: #073763; font-family: Comic Sans MS;"><br /></span>
<span style="color: #073763; font-family: Comic Sans MS;">Those are the 3D models which they will appear when Metaio SDK Detect the scene.</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">(pointing the camera to any scene Metaio will try to identify it)</span><br />
<span style="color: #073763; font-family: Comic Sans MS;"><br /></span>
<span style="color: #073763; font-family: Comic Sans MS;">The model "Max" can be controlled with the keyboard:&nbsp;Movement&nbsp;(W,S,A,D)</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">The model "Tyrant Zombie" is associated with an AI system</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">if model "Max" get to his sight range he will start following him</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">if model "Max" get to his attack range he will start attacking him</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">(at this sample no health system is not applied)</span><br />
<span style="color: #073763; font-family: Comic Sans MS;"><br /></span>
<span style="color: #073763; font-family: Comic Sans MS;">-Running the sample:</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">click on the play button at Unity</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">-Then click on the 3D button (it will appear at the left side of the screen)</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">-Then move the camera to the right or to the right then to left (some times you may need to click on 3D button again)</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">-Then once Metaio detect the scene you will see "Max" &amp; the "Tyrant Zombie" appear on the screen</span><br />
<span style="color: #073763; font-family: Comic Sans MS;"><br /></span>
<span style="color: #073763; font-family: Comic Sans MS;">-Note: I used UnityVS add in to make edit and view the code in Visual Studio</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">You may have to go to:"Edit/Preferences" "External Tools" "External Script Editor"</span><br />
<span style="color: #073763; font-family: Comic Sans MS;">to choose your preferred Editor.</span><br />
<span style="color: #073763; font-family: Comic Sans MS;"><br /></span>
<span style="color: #073763; font-family: Comic Sans MS;"><br /></span>
<span style="color: #073763;"><span style="font-family: Comic Sans MS;"><span style="font-size: small;"></span></span></span><br />
<br /></div>
</div>
<a href="http://www.codeproject.com/script/Articles/BlogFeedList.aspx?amid=1227574" rel="tag"> This Article is also shared on CodeProject</a></div>
