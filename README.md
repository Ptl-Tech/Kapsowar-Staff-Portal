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

Check the output folder contains `App.Server.dll`, `web.config` and `wwwroot\index.html` (with `wwwroot\assets\`). If `wwwroot` is missing, run `npm run build` in `app.client` and copy the contents of `app.client\dist` into `wwwroot`.

### 3. Create the IIS site

1. Copy the publish output to the server, e.g. `C:\inetpub\KapsowarStaffPortal`.
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
