--require 'FairyGUI'

testFairyGUI = {
	
}

function testFairyGUI.init()
	UIPackage.AddPackage("FairyGUI/Package1");
	local uiPanelObj = UnityEngine.GameObject.Find("UIPanel");
    if uiPanelObj ~= nil then 
    	print("1111111111111111111111");
    	local uiPanel = uiPanelObj:GetComponent(typeof(UIPanel));
    	if uiPanel ~= nil then 
    		print("2222222222222222222");
    		local main_component = uiPanel.ui;
            if main_component ~= nil then 
            	local list = main_component:GetChild("List");
            	if list then 
            		local aImage = UIPackage.CreateObject("Package1", "bg_title");
            		if aImage then 
            			print("3333333333333333");
            			list:AddChild(aImage);
            		end
            	end
            end

    	end
    end

end

testFairyGUI.init();