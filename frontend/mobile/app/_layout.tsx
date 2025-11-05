import { Stack } from 'expo-router'
import { StatusBar } from 'expo-status-bar'

export default function RootLayout() {
  return (
    <>
      <StatusBar style="auto" />
      <Stack>
        <Stack.Screen name="index" options={{ title: 'Blitz Prive' }} />
        <Stack.Screen name="services" options={{ title: 'Services' }} />
      </Stack>
    </>
  )
}
