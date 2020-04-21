# ccm-examples
Examples of using - Idea StatiCa Code Check Manager API

It is requiered :
<ol>
  <li>To have IdeaStatica v20.0 installed on the PC</li>
  <li>To set the path to IdeaStatiCa Code Check Manager (IdeaCodeCheck.exe) in the project settings (or in the config file)</li>
</ol>


In the file *FEAppExample_1.exe.config* should be the correct path

```xml
        <FEAppExample_1.Properties.Settings>
            <setting name="IdeaStatiCaDir" serializeAs="String">
                <value>C:\Program Files\IDEA StatiCa\StatiCa 20\IdeaCodeCheck.exe</value>
          </setting>
```
