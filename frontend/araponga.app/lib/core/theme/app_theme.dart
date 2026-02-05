import 'package:flutter/material.dart';

import '../config/constants.dart';

/// Tema Ará: clean, leve, harmônico. Fundo escuro como padrão para conforto visual.
/// Usa [AppConstants] para radius e espaçamento (26_FLUTTER_DESIGN_GUIDELINES).
class AppTheme {
  AppTheme._();

  static const Color _primary = Color(0xFF81C784);
  static const Color _surfaceDark = Color(0xFF121212);
  static const Color _surfaceVariant = Color(0xFF1E1E1E);
  static const Color _onSurfaceDark = Color(0xFFE8E8E8);
  static const Color _onSurfaceVariant = Color(0xFFB0B0B0);
  static const Color _outline = Color(0xFF404040);

  /// Tema claro (opcional).
  static ThemeData get light {
    return ThemeData(
      useMaterial3: true,
      brightness: Brightness.light,
      colorScheme: ColorScheme.light(
        primary: const Color(0xFF1B5E20),
        surface: const Color(0xFFFAFAFA),
        onSurface: const Color(0xFF1C1C1C),
      ),
      appBarTheme: const AppBarTheme(centerTitle: true, elevation: 0),
      bottomNavigationBarTheme: const BottomNavigationBarThemeData(
        type: BottomNavigationBarType.fixed,
      ),
      snackBarTheme: SnackBarThemeData(
        behavior: SnackBarBehavior.floating,
        insetPadding: const EdgeInsets.symmetric(horizontal: 16),
        contentTextStyle: const TextStyle(fontSize: 15),
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppConstants.radiusSm),
        ),
      ),
      filledButtonTheme: FilledButtonThemeData(
        style: FilledButton.styleFrom(
          minimumSize: const Size(88, AppConstants.minTouchTargetSize),
          padding: const EdgeInsets.symmetric(horizontal: AppConstants.spacingLg, vertical: AppConstants.spacingSm),
        ),
      ),
      textButtonTheme: TextButtonThemeData(
        style: TextButton.styleFrom(
          minimumSize: const Size(64, AppConstants.minTouchTargetSize),
          padding: const EdgeInsets.symmetric(horizontal: AppConstants.spacingMd),
        ),
      ),
    );
  }

  /// Tema escuro (padrão): fundo escuro, conteúdo leve e harmônico.
  static ThemeData get dark {
    return ThemeData(
      useMaterial3: true,
      brightness: Brightness.dark,
      colorScheme: ColorScheme.dark(
        primary: _primary,
        surface: _surfaceDark,
        onSurface: _onSurfaceDark,
        surfaceContainerHighest: _surfaceVariant,
        onSurfaceVariant: _onSurfaceVariant,
        outline: _outline,
      ),
      scaffoldBackgroundColor: _surfaceDark,
      appBarTheme: const AppBarTheme(
        centerTitle: true,
        elevation: 0,
        scrolledUnderElevation: 0.5,
        backgroundColor: _surfaceDark,
        foregroundColor: _onSurfaceDark,
      ),
      bottomNavigationBarTheme: const BottomNavigationBarThemeData(
        type: BottomNavigationBarType.fixed,
        selectedItemColor: _primary,
        unselectedItemColor: _onSurfaceVariant,
        backgroundColor: _surfaceDark,
      ),
      cardTheme: CardTheme(
        color: _surfaceVariant,
        elevation: 0,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppConstants.radiusMd),
        ),
      ),
      snackBarTheme: SnackBarThemeData(
        behavior: SnackBarBehavior.floating,
        insetPadding: const EdgeInsets.symmetric(horizontal: 16),
        contentTextStyle: const TextStyle(fontSize: 15),
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppConstants.radiusSm),
        ),
      ),
      iconButtonTheme: IconButtonThemeData(
        style: IconButton.styleFrom(
          minimumSize: const Size(AppConstants.minTouchTargetSize, AppConstants.minTouchTargetSize),
          tapTargetSize: MaterialTapTargetSize.shrinkWrap,
        ),
      ),
      filledButtonTheme: FilledButtonThemeData(
        style: FilledButton.styleFrom(
          minimumSize: const Size(88, AppConstants.minTouchTargetSize),
          padding: const EdgeInsets.symmetric(horizontal: AppConstants.spacingLg, vertical: AppConstants.spacingSm),
        ),
      ),
      textButtonTheme: TextButtonThemeData(
        style: TextButton.styleFrom(
          minimumSize: const Size(64, AppConstants.minTouchTargetSize),
          padding: const EdgeInsets.symmetric(horizontal: AppConstants.spacingMd),
        ),
      ),
    );
  }
}
