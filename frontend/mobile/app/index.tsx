import { View, Text, StyleSheet, TouchableOpacity, ScrollView } from 'react-native'
import { Link } from 'expo-router'
import { LinearGradient } from 'expo-linear-gradient'

export default function Home() {
  return (
    <ScrollView style={styles.container}>
      <LinearGradient
        colors={['#3b82f6', '#60a5fa', '#93c5fd']}
        style={styles.header}
      >
        <Text style={styles.demo}>Demo Tenant: Blitz Prive</Text>
        <Text style={styles.title}>Welcome to Blitz Prive</Text>
        <Text style={styles.subtitle}>
          Premium wellness services from the comfort of your home
        </Text>
      </LinearGradient>

      <View style={styles.content}>
        <View style={styles.actions}>
          <Link href="/services" asChild>
            <TouchableOpacity style={styles.primaryButton}>
              <Text style={styles.primaryButtonText}>Browse Services</Text>
            </TouchableOpacity>
          </Link>
          <Link href="/auth/register" asChild>
            <TouchableOpacity style={styles.secondaryButton}>
              <Text style={styles.secondaryButtonText}>Sign Up</Text>
            </TouchableOpacity>
          </Link>
        </View>

        <View style={styles.features}>
          <View style={styles.featureCard}>
            <Text style={styles.featureIcon}>💆</Text>
            <Text style={styles.featureTitle}>Massage Therapy</Text>
            <Text style={styles.featureDescription}>
              Relax with professional massage services at your location
            </Text>
          </View>
          <View style={styles.featureCard}>
            <Text style={styles.featureIcon}>🏃</Text>
            <Text style={styles.featureTitle}>Physiotherapy</Text>
            <Text style={styles.featureDescription}>
              Recovery and wellness with certified physiotherapists
            </Text>
          </View>
          <View style={styles.featureCard}>
            <Text style={styles.featureIcon}>✨</Text>
            <Text style={styles.featureTitle}>Facial & Spa</Text>
            <Text style={styles.featureDescription}>
              Rejuvenate with premium facial and spa treatments
            </Text>
          </View>
        </View>

        <View style={styles.howItWorks}>
          <Text style={styles.sectionTitle}>How It Works</Text>
          <View style={styles.steps}>
            {[
              { num: 1, title: 'Choose Service', desc: 'Browse our wellness services' },
              { num: 2, title: 'Select Provider', desc: 'Choose your preferred provider' },
              { num: 3, title: 'Pick Time Slot', desc: 'Book your preferred time' },
              { num: 4, title: 'Enjoy Service', desc: 'We come to you' },
            ].map((step) => (
              <View key={step.num} style={styles.step}>
                <Text style={styles.stepNumber}>{step.num}</Text>
                <Text style={styles.stepTitle}>{step.title}</Text>
                <Text style={styles.stepDesc}>{step.desc}</Text>
              </View>
            ))}
          </View>
        </View>
      </View>
    </ScrollView>
  )
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
  },
  header: {
    padding: 40,
    paddingTop: 60,
    alignItems: 'center',
  },
  demo: {
    fontSize: 12,
    color: '#e0f2fe',
    marginBottom: 8,
    textAlign: 'center',
  },
  title: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 16,
    textAlign: 'center',
  },
  subtitle: {
    fontSize: 18,
    color: '#fff',
    textAlign: 'center',
  },
  content: {
    padding: 20,
  },
  actions: {
    gap: 12,
    marginBottom: 32,
  },
  primaryButton: {
    backgroundColor: '#2563eb',
    padding: 16,
    borderRadius: 8,
    alignItems: 'center',
  },
  primaryButtonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: '600',
  },
  secondaryButton: {
    backgroundColor: '#fff',
    padding: 16,
    borderRadius: 8,
    alignItems: 'center',
    borderWidth: 2,
    borderColor: '#2563eb',
  },
  secondaryButtonText: {
    color: '#2563eb',
    fontSize: 18,
    fontWeight: '600',
  },
  features: {
    gap: 16,
    marginBottom: 32,
  },
  featureCard: {
    backgroundColor: '#f9fafb',
    padding: 20,
    borderRadius: 12,
    alignItems: 'center',
  },
  featureIcon: {
    fontSize: 48,
    marginBottom: 12,
  },
  featureTitle: {
    fontSize: 20,
    fontWeight: '600',
    marginBottom: 8,
  },
  featureDescription: {
    fontSize: 14,
    color: '#6b7280',
    textAlign: 'center',
  },
  howItWorks: {
    marginBottom: 32,
  },
  sectionTitle: {
    fontSize: 28,
    fontWeight: 'bold',
    marginBottom: 24,
    textAlign: 'center',
  },
  steps: {
    gap: 16,
  },
  step: {
    backgroundColor: '#f9fafb',
    padding: 20,
    borderRadius: 12,
    alignItems: 'center',
  },
  stepNumber: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#2563eb',
    marginBottom: 8,
  },
  stepTitle: {
    fontSize: 18,
    fontWeight: '600',
    marginBottom: 4,
  },
  stepDesc: {
    fontSize: 14,
    color: '#6b7280',
    textAlign: 'center',
  },
})
