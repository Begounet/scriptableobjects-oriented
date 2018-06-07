# ScriptableObjects Oriented

Provide tools for an Unity architecture based on ScriptableObjects (SO).

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