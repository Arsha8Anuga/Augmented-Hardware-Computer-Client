# Setup Guide – Unity Project with Vuforia

Follow the steps below to properly set up this project:

---

### 1. Clone Repository

Clone this repository to your local machine:

```bash
git clone Augmented-Hardware-Computer-Client
```

---

### 2. Download Vuforia SDK

Download the Vuforia SDK (`.unitypackage`) from the official website:
https://developer.vuforia.com/

---

### 3. Open Project in Unity

* Open **Unity Hub**
* Select **"Add project from disk"**
* Choose the cloned project folder

---

### 4. Enter Safe Mode

After opening the project, Unity may enter **Safe Mode**.
Wait until the loading process completes.

---

### 5. Import Vuforia Package

* Drag and drop the downloaded `.unitypackage` into the Unity Editor
* Complete the import process

---

### 6. Locate Migration Files

Using File Explorer, navigate to:

```
<project-folder>/Assets/Editor/Migration/
```

---

### 7. Move Vuforia Package

* Find the `.tgz` file inside the `Migration` folder
* Copy it to:

```
<project-folder>/Packages/
```

---

### 8. Exit Safe Mode

Return to Unity and exit Safe Mode.
The project should now function correctly with Vuforia.

---

### Notes

* Ensure the `.tgz` file name matches the one referenced in `manifest.json`
* Do not rename the file unless you also update the manifest

---

### Support

If you encounter any issues, please contact the repository owner.
