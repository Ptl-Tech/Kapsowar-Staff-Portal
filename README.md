# Kapsowar Staff Portal

ASP.NET Core 8 API (`App.Server`) with a Vue 3 + Vite front end (`app.client`). Publishing `App.Server` also builds the Vue app and places it in `wwwroot`, so the whole portal deploys as a single IIS site.

## Publishing to IIS

### 1. Server setup (once per server)

1. Enable **IIS** (Windows Features → Internet Information Services, or Server Manager → Add Roles → Web Server (IIS) on Windows Server).
2. Install the **ASP.NET Core 8.0 Hosting Bundle** from https://dotnet.microsoft.com/download/dotnet/8.0.
3. Restart IIS from an elevated prompt:
   ```
   net stop was /y
   net start w3svc
   ```

### 2. Build the publish output (dev machine)

Requires the .NET 8 SDK and Node.js/npm (the publish runs `npm run build` for `app.client`).

**Visual Studio:** right-click **App.Server** → **Publish** → choose the `FolderProfile1` profile (or create a new Folder profile) → **Publish**.

**Command line:**
```
cd App.Server
dotnet publish -c Release -o C:\publish\KapsowarStaffPortal
```

**Where the output goes:**

| Publish method | Output folder |
| --- | --- |
| Command line | The path passed to `-o` (above: `C:\publish\KapsowarStaffPortal`) |
| Visual Studio, `FolderProfile1` | The `PublishUrl` in `App.Server/Properties/PublishProfiles/FolderProfile1.pubxml` (currently `C:\Work\Other\ptl\Kapsowar\Publishes\KapsowarStaffPortal`; edit it to suit your machine) |
| Visual Studio, new Folder profile | The folder you chose when creating the profile (also shown at the end of the publish) |

The output folder should look like this:

```
KapsowarStaffPortal\
  App.Server.dll
  web.config
  appsettings.json
  ...other .dll files
  wwwroot\
    index.html
    assets\
```

If `wwwroot` is missing or empty, run `npm run build` in `app.client` and copy the contents of `app.client\dist` into `wwwroot`.

### 3. Create the IIS site

1. Copy the **contents** of the output folder to the server, e.g. `C:\inetpub\KapsowarStaffPortal`.

   Any folder works, including `C:\inetpub\wwwroot`:
   - `C:\inetpub\KapsowarStaffPortal` or `C:\inetpub\wwwroot\KapsowarStaffPortal` as its **own site** (steps below) — recommended.
   - Directly in `C:\inetpub\wwwroot`, replacing the Default Web Site: delete the default `iisstart.htm`/`iisstart.png`, then set the Default Web Site's application pool to **No Managed Code** instead of creating a new site.
   - Do **not** put it in a subfolder of `C:\inetpub\wwwroot` and "Convert to Application" under the Default Web Site — the app would run at `/KapsowarStaffPortal/`, which it isn't set up for (see Notes).
2. **Application Pools → Add Application Pool**
   - Name: `KapsowarStaffPortal`
   - .NET CLR version: **No Managed Code**
   - Managed pipeline mode: Integrated
3. **Sites → Add Website**
   - Physical path: `C:\inetpub\KapsowarStaffPortal`
   - Application pool: `KapsowarStaffPortal`
   - Bindings: https with a certificate (recommended — the app uses `UseHttpsRedirection`), plus http if needed.
4. Grant the app pool read access to the folder:
   ```
   icacls "C:\inetpub\KapsowarStaffPortal" /grant "IIS AppPool\KapsowarStaffPortal:(OI)(CI)RX"
   ```
   Folders under `C:\inetpub\wwwroot` are already readable by IIS, so this step can usually be skipped there.
5. Browse to the site.

### 4. Updating an existing deployment

1. Publish again (step 2).
2. Stop the site's app pool (or drop an `app_offline.htm` file in the site folder) so IIS releases `App.Server.dll`.
3. Copy the new files over the old ones. Keep any server-specific `appsettings.json` changes.
4. Start the app pool (or delete `app_offline.htm`).

### Notes

- **Sessions are held in memory.** Keep the app pool's *Maximum Worker Processes* at `1`. A recycle or idle shutdown logs users out, so consider setting *Idle Time-out* to `0` and *Start Mode* to `AlwaysRunning`.
- **Backend services:** the server must be able to reach the services under `App.Server/Connected Services`. If they use Windows authentication, the app pool identity needs access.
- **Host as its own site**, not a sub-application (e.g. `/staffportal`). A sub-path requires setting `base` in `app.client/vite.config.js` and updating the router and API URLs.

### Troubleshooting

Enable logging by setting `stdoutLogEnabled="true"` in the deployed `web.config` and creating a `logs` folder beside it, then reload the page and check the log.

| Error | Likely cause |
| --- | --- |
| HTTP 500.19 | Hosting Bundle not installed — install it and restart IIS |
| HTTP 500.30 / 500.31 | App failed on startup or .NET 8 runtime missing — see stdout log |
| 403 or blank page | Folder permissions, or `wwwroot\index.html` missing |
