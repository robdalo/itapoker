const { app, BrowserWindow, ipcMain } = require('electron');
const path = require('path');

function createWindow () {
  const win = new BrowserWindow({
    width: 900,
    height: 800,
    webPreferences: { 
        nodeIntegration: true,
        contextIsolation: false
    }
  });

  win.loadFile(path.join(__dirname, 'dist/itapoker/browser/index.html'));
  win.removeMenu();
  win.webContents.setZoomFactor(1.0);
}

ipcMain.on('quit-app', () => {
  app.quit();
});

app.whenReady().then(createWindow);