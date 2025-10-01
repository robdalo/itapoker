ng build --base-href ./
npx electron-packager . itapoker --platform=win32 --arch=x64 --out=release --overwrite
npm run electron
