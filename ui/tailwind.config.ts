import type { Config } from 'tailwindcss';

export default {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      dropShadow: {
        glow: [
          "0 0px 10px rgba(255, 255, 255, 0.35)",
          "0 0px 65px rgba(255, 255, 255, 0.2)",
        ],
      }
    },
  },
  plugins: [],
} satisfies Config;
