# ScriptableObjects Oriented

Provide tools for an Unity architecture based on ScriptableObjects (SO).<br>
The philosophy of this plugin is based on that [conference](https://www.youtube.com/watch?v=raQ3iHhE_Kk).

## Create a SO Event
A SO Event is a ScriptableObject that can raise event.<br>
Since the event is an asset with a static number of parameter, you will probably create a lot of SO event asset.

Create a SO Event from scratch just to change the type of parameter or add new one of your event can be redundant and time-consuming, a little wizard is provided to help you.

- Assets -> Create -> SO -> Create -> Event
- The wizard will appear
- Set the event name. It will define the class name.
- [Optional] Set the category name. It will define its location in the "Create" asset menu.
- [Optional] Set the number of parameters
- Set the parameter name
- Set the parameter type name OR use the preset box to select common types so you don't have to write it.
- Click "Create" when you are done
- Let Unity compile and you should now be able to create a new event from *Assets -> Create -> SO -> [Your Category] -> [Your Event Name]*

You can add more types with the settings.

## Settings
The settings allow to improve the SO Event Wizard.<br>
Start by creating a new one.
- Assets -> Create -> SO -> Create -> Settings
- Add additional types
- Open the SO Event Wizard and link the settings
- Your new types are now available among the presets

## Using SO Events

Just declare your SO Event in your class, as it is a standard C# class.<br>
You can dynamically add listener, remove listener and raise event, like in any standard event system.<br>
However, now you can select the SO Event in the *Project* tab and you will see that you have additional informations.<br>
All these data are editor-only so no overhead in your final build.<br>
You can :
- Add a description : you can give extra datas to explain where it should be called.
- Set some parameters : these parameters will be used when you click on **Raise**
- Raise the event : it will trigger the event using the above parameters
Raising the event is really useful to manually test isolated system.

### SOEventMode Attribute

You can add a `SOEventMode` attribute on your event field to explicitly tell to the world if your event will be a listener or a broadcaster.<br>
It will allow you to be more explicit on the use of an event, visible from the Unity Inspector, next to the event field.
- Broadcaster : ![Broadcaster icon](Editor/Resources/IconBroadcaster.png)
- Listener : ![Listener icon](Editor/Resources/IconListener.png)

Examples :

`[SOEventMode(SOEventActionMode.Broadcaster)]`<br>
`public SOMyEvent eventBroadcaster;`

`[SOEventMode(SOEventActionMode.Listener)]`<br>
`public SOMyEvent eventListener;`