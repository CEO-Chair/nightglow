using System;
using Gtk;

namespace Nightglow.UI;

public class LauncherWindow : ApplicationWindow {
    public LauncherWindow(Gio.Application application) {
        var rootBox = new Box { Name = "rootBox" };
        rootBox.SetOrientation(Orientation.Vertical);
        this.SetChild(rootBox);

        var centerBox = new Box { Name = "centerBox" };
        var instancePane = new InstancePane();
        var instanceFlow = new FlowBox { Name = "instanceFlow" };

        var ribbonBox = new Box { Name = "ribbonBox" };
        ribbonBox.SetOrientation(Orientation.Horizontal);
        ribbonBox.SetVexpandSet(true);
        var addInstanceButton = new Button { Label = "Add Instance" };
        addInstanceButton.OnClicked += (Button _, EventArgs _) => {
            var addInstanceWindow = new AddInstanceWindow(application, instancePane, instanceFlow);
            addInstanceWindow.Show();
        };
        ribbonBox.Append(addInstanceButton);
        ribbonBox.Append(new Button { Label = "Folders" });
        ribbonBox.Append(new Button { Label = "Settings" });

        var helpButton = new Button { Label = "Help" };
        helpButton.OnClicked += (Button sender, EventArgs args) => { Common.Launcher.Platform.OpenUrl("https://github.com/steviegt6/terraprisma"); };
        ribbonBox.Append(helpButton);
        rootBox.Append(ribbonBox);

        centerBox.SetOrientation(Orientation.Horizontal);

        centerBox.Append(instanceFlow);

        instanceFlow.SetValign(Gtk.Align.Fill);
        instanceFlow.SetVexpand(true);
        instanceFlow.SetHexpand(true);
        foreach (var instance in Program.Launcher.Instances) {
            instanceFlow.Append(new UIInstance(instance, instancePane)); // Needs to be sorted eventually
        }
        instancePane.SetParent(centerBox);

        var uiInstance = instanceFlow.GetChildAtIndex(0);
        if (uiInstance != null)
            instancePane.SetInstance((UIInstance)uiInstance);
        rootBox.Append(centerBox);

        this.Application = (Application)application;
        this.Title = "Nightglow";
        this.SetDefaultSize(800, 600);
    }
}