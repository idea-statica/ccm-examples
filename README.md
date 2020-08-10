## ccm-examples
Examples of using - Idea StatiCa Code Check Manager API

 ### Example [FEAppExample_1](https://github.com/idea-statica/ccm-examples/tree/master/FEAppExample_1)
 shows how to connect IDEA CCM (IDEA Code Check Manager) to any FEA or CAD application. It uses [IdeaStatiCa.Plugin](https://github.com/idea-statica/ideastatica-plugin) for communication with *IdeaCodeCheck.exe*.

### ![CCM + FakeFEA](https://github.com/idea-statica/ccm-examples/blob/master/Images/fake-fea.png?raw=true)

It is requiered :
<ol>
  <li>To have IdeaStatica v20.0 installed on the PC. Free trial version version can be obtained [here](https://www.ideastatica.com/free-trial).</li>
  <li>To set the path to IdeaStatiCa Code Check Manager (IdeaCodeCheck.exe) in the project settings (or in the config file)</li>
</ol>


In the file *FEAppExample_1.exe.config* should be the correct path

```xml
        <FEAppExample_1.Properties.Settings>
            <setting name="IdeaStatiCaDir" serializeAs="String">
                <value>C:\Program Files\IDEA StatiCa\StatiCa 20\IdeaCodeCheck.exe</value>
          </setting>
```

When *Some FE Application* starts model should be created - it can be done creating default model - see the button **Default** or be opening model from xml - see the button **Load model**. IDEA CCM opens by clicking on the button **Run IDEA StatiCa CCM**.
It is possible to get all materials and cross-sections in an open project as well as in IDEA StatiCa MPRL.

In file [member_project.xml](https://github.com/idea-statica/ccm-examples/blob/master/FEAppExample_1/member_project.xml) there is example how to import member into CCM.

### ![Member in CCM](https://github.com/idea-statica/ccm-examples/blob/master/Images/member-project.png?raw=true)

In the test application do following :
* Set path to _IdeaCodeCheck.exe_
* Build the project _FEAppExample_1.csproj_
* Run _FEAppExample_1.exe_
* Load model _member_project.xml_
* Click on the button _Run IDEA StatiCa CCM_
* Create a new project - data will be stored in the folder _C:\Users\YOUR-USER-NAME\Documents\FEAppExample_1\member_project\IdeaStatiCa-member_project_
* Import member by clicking in the button **Member**
* In CCM configure the connections #2 and #3
* Configure the memeber #2
* Open the column #2 by clicking on the button **Open**
