import Link from 'next/link'

export default function Home() {
  return (
    <main className="min-h-screen bg-gradient-to-b from-blue-50 to-white">
      <div className="container mx-auto px-4 py-16">
        <div className="text-center mb-12">
          <h1 className="text-5xl font-bold text-gray-900 mb-4">
            Welcome to Blitz Prevair
          </h1>
          <p className="text-xl text-gray-600 mb-8">
            Book wellness services from the comfort of your home
          </p>
          <div className="flex justify-center gap-4">
            <Link
              href="/services"
              className="px-8 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
            >
              Browse Services
            </Link>
            <Link
              href="/auth/register"
              className="px-8 py-3 bg-white text-blue-600 border-2 border-blue-600 rounded-lg hover:bg-blue-50 transition"
            >
              Sign Up
            </Link>
          </div>
        </div>

        <div className="grid md:grid-cols-3 gap-8 mt-16">
          <div className="bg-white p-6 rounded-lg shadow-md">
            <div className="text-4xl mb-4">💆</div>
            <h3 className="text-xl font-semibold mb-2">Massage Therapy</h3>
            <p className="text-gray-600">
              Relax with professional massage services at your location
            </p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow-md">
            <div className="text-4xl mb-4">🏃</div>
            <h3 className="text-xl font-semibold mb-2">Physiotherapy</h3>
            <p className="text-gray-600">
              Recovery and wellness with certified physiotherapists
            </p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow-md">
            <div className="text-4xl mb-4">✨</div>
            <h3 className="text-xl font-semibold mb-2">Facial & Spa</h3>
            <p className="text-gray-600">
              Rejuvenate with premium facial and spa treatments
            </p>
          </div>
        </div>

        <div className="mt-16 text-center">
          <h2 className="text-3xl font-bold text-gray-900 mb-4">
            How It Works
          </h2>
          <div className="grid md:grid-cols-4 gap-6 mt-8">
            <div className="bg-white p-6 rounded-lg">
              <div className="text-3xl font-bold text-blue-600 mb-2">1</div>
              <h4 className="font-semibold mb-2">Choose Service</h4>
              <p className="text-sm text-gray-600">
                Browse and select from our range of wellness services
              </p>
            </div>
            <div className="bg-white p-6 rounded-lg">
              <div className="text-3xl font-bold text-blue-600 mb-2">2</div>
              <h4 className="font-semibold mb-2">Select Provider</h4>
              <p className="text-sm text-gray-600">
                Choose your preferred provider or gender preference
              </p>
            </div>
            <div className="bg-white p-6 rounded-lg">
              <div className="text-3xl font-bold text-blue-600 mb-2">3</div>
              <h4 className="font-semibold mb-2">Pick Time Slot</h4>
              <p className="text-sm text-gray-600">
                View available times and book your preferred slot
              </p>
            </div>
            <div className="bg-white p-6 rounded-lg">
              <div className="text-3xl font-bold text-blue-600 mb-2">4</div>
              <h4 className="font-semibold mb-2">Enjoy Service</h4>
              <p className="text-sm text-gray-600">
                Relax while our professionals come to you
              </p>
            </div>
          </div>
        </div>
      </div>
    </main>
  )
}
