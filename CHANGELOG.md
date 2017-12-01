# RatherWeird Changelog

## 0.4.1 [2017-12-01]
### Changed
* Improved RA3 detection - if you restart RA3 or start the tool after RA3, RA3 will be detected faster and work directly.

## 0.4.0 [2017-11-12]
### Added
* Support to disable Windows Keys (left and right).

## 0.3.0 [2017-11-10]
### Added
* Swap the default healthbar behavior. Now automatically shows them if desired.

### Fixed
* RA3 process is launched in a Thread, making the UI *NOT* freeze.

## 0.2.0 [2017-06-03]
### Stuff Fixed
* [[`c85a6d21`](https://github.com/zokker13/RatherWeird/commit/c85a6d21052386bb3a44eb3f2d583c66f5a14935)] - Make [ALT UP] actually work (it seemed to not work - I now invoke it exactly as the game does and added a tiny timeout of 100ms)

### Stuff Added
* [[`48391be2`](https://github.com/zokker13/RatherWeird/commit/48391be29f471609d4875dccb63160db484ce07a)] - Hook [Numpad ENTER] and invoke the normal [ENTER]. RA3 won't allow the use of the [Numpad ENTER].
* [[`0d2a0bf6`](https://github.com/zokker13/RatherWeird/commit/0d2a0bf6e413219758218f9dd813aa75f59a4dd5)] - The Lock Cursor feature now worls with windows with borders EXCLUDING the borders.

## 0.1.0
### Stuff Added
* Lock your cursor to RA3
* Remove border of RA3 (when started in windowed mode)
* Start RA3 in window mode
* Invoke [ALT UP] when you go back in game (this fixes a bug that caused the game to think your [ALT] was always pressed so you couldn't use your ingame hotkeys )
