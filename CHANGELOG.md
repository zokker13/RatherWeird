# RatherWeird Changelog

## ??
### Stuff Fixed
* Make [ALT UP] actually work (it seemed to not work - I now invoke it exactly as the game does and added a tiny timeout of 100ms) [c85a6d21](https://github.com/zokker13/RatherWeird/commit/c85a6d21052386bb3a44eb3f2d583c66f5a14935)

### Stuff Added
* Hook [Numpad ENTER] and invoke the normal [ENTER]. RA3 won't allow the use of the [Numpad ENTER]. [48391be2](https://github.com/zokker13/RatherWeird/commit/48391be29f471609d4875dccb63160db484ce07a)
* The Lock Cursor feature now worls with windows with borders EXCLUDING the borders. [0d2a0bf6](https://github.com/zokker13/RatherWeird/commit/0d2a0bf6e413219758218f9dd813aa75f59a4dd5)

## 0.1.0
### Stuff Added
* Lock your cursor to RA3
* Remove border of RA3 (when started in windowed mode)
* Start RA3 in window mode
* Invoke [ALT UP] when you go back in game (this fixes a bug that caused the game to think your [ALT] was always pressed so you couldn't use your ingame hotkeys )